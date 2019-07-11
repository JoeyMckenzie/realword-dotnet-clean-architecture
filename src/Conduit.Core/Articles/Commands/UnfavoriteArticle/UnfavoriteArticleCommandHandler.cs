namespace Conduit.Core.Articles.Commands.UnfavoriteArticle
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Persistence;
    using Persistence.Infrastructure;

    public class UnfavoriteArticleCommandHandler : IRequestHandler<UnfavoriteArticleCommand, ArticleViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public UnfavoriteArticleCommandHandler(ICurrentUserContext currentUserContext, ConduitDbContext context, IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModel> Handle(UnfavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the article and invalidate the request if not found
            var articleFromSlug = await _context.FirstArticleOrDefaultWithRelatedEntities(request.Slug, cancellationToken);

            if (articleFromSlug == null)
            {
                throw new ConduitApiException($"Article [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Retrieve the current user
            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Remove the favorite, if the user has favorited the article
            var existingFavorite = articleFromSlug.Favorites.FirstOrDefault(f => f.User == currentUser);
            if (existingFavorite != null)
            {
                _context.Remove(existingFavorite);
                await _context.SaveChangesAsync(cancellationToken);
            }

            // Map and return the article
            var articleViewModel = new ArticleViewModel
            {
                Article = _mapper.Map<ArticleDto>(articleFromSlug)
            };
            articleViewModel.SetViewModelPropertiesForArticle(articleViewModel.Article, articleFromSlug, articleFromSlug.ArticleTags.ToList(), currentUser);

            return articleViewModel;
        }
    }
}