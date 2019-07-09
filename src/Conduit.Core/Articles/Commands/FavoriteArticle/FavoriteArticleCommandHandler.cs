namespace Conduit.Core.Articles.Commands.FavoriteArticle
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Persistence;
    using Persistence.Infrastructure;
    using Shared;

    public class FavoriteArticleCommandHandler : IRequestHandler<FavoriteArticleCommand, ArticleViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTime _dateTime;

        public FavoriteArticleCommandHandler(ICurrentUserContext currentUserContext, ConduitDbContext context, IMapper mapper, IDateTime dateTime)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<ArticleViewModel> Handle(FavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the article
            var articleFromSlug = await _context.FirstArticleOrDefaultWithRelatedEntities(request.Slug, cancellationToken);

            if (articleFromSlug == null)
            {
                throw new ConduitApiException($"Article with slug [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Create the user favorite, if the user has not already favorited the article
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            if (articleFromSlug.Favorites.All(f => f.User != currentUser))
            {
                articleFromSlug.Favorites.Add(new Favorite
                {
                    User = currentUser,
                    Article = articleFromSlug,
                    CreatedAt = _dateTime.Now,
                    UpdatedAt = _dateTime.Now
                });

                // Increment the favorites count
                articleFromSlug.FavoritesCount++;

                await _context.SaveChangesAsync(cancellationToken);
            }

            // Instantiate and return the view model
            var articleViewModel = new ArticleViewModel
            {
                Article = _mapper.Map<ArticleDto>(articleFromSlug)
            };
            articleViewModel.SetViewModelPropertiesForArticle(articleViewModel.Article, articleFromSlug, articleFromSlug.ArticleTags.ToList(), currentUser);

            return articleViewModel;
        }
    }
}