namespace Conduit.Core.Articles.Queries.GetArticle
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Extensions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleViewModel>
    {
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetArticleQueryHandler(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModel> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
            var articleFromSlug = await _context.Articles
                .Include(a => a.Author)
                    .ThenInclude(au => au.Followers)
                .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.Favorites)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase), cancellationToken);

            if (articleFromSlug == null)
            {
                throw new ConduitApiException($"Article with slug [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Map the results
            var articleViewModel = new ArticleViewModel
            {
                Article = _mapper.Map<ArticleDto>(articleFromSlug)
            };
            articleViewModel.SetViewModelPropertiesForArticle(articleViewModel.Article, articleFromSlug, articleFromSlug.ArticleTags.ToList(), null);

            return articleViewModel;
        }
    }
}