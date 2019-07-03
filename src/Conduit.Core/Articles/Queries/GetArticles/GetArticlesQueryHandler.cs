namespace Conduit.Core.Articles.Queries.GetArticles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Articles;
    using Domain.Entities;
    using Domain.ViewModels;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, ArticleViewModelList>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetArticlesQueryHandler(
            UserManager<ConduitUser> userManager,
            ConduitDbContext context,
            IMapper mapper,
            ICurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModelList> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all articles from the database and include the corresponding
            var articles = _context.Articles
                .Include(a => a.Author)
                    .ThenInclude(au => au.Followers)
                .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.Favorites)
                    .ThenInclude(f => f.User)
                .AsQueryable();

            // Instantiate the user and empty results list
            ConduitUser author = null;
            var noSearchResults = new ArticleViewModelList
            {
                Articles = new List<ArticleDto>()
            };

            // Filter on author
            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                author = await _userManager.FindByNameAsync(request.Author);

                // If no author is found during the search, return an empty list
                if (author != null)
                {
                    articles = articles.Where(a => a.Author == author);
                }
                else
                {
                    return noSearchResults;
                }
            }

            // Filter on tags
            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                var tag = await _context.Tags
                    .FirstOrDefaultAsync(t => string.Equals(t.Description, request.Tag, StringComparison.OrdinalIgnoreCase), cancellationToken);

                // If no tag is found for the requesting tag, return an empty list
                if (tag != null)
                {
                    articles = articles.Where(a => a.ArticleTags.Select(at => at.Tag).Contains(tag));
                }
                else
                {
                    return noSearchResults;
                }
            }

            // Filter on favorited
            if (!string.IsNullOrWhiteSpace(request.Favorited))
            {
                // If not favorited articles on found by the user, return an empty list
                if (articles.Any(a => a.Favorites.Select(f => f.User).Contains(author)))
                {
                    articles = articles.Where(a => a.Favorites.Select(f => f.User).Contains(author));
                }
                else
                {
                    return noSearchResults;
                }
            }

            // Paginate and map the results
            var results = await articles
                .Skip(request.Offset)
                .Take(request.Limit)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);

            var articlesViewModelList = new ArticleViewModelList
            {
                Articles = _mapper.Map<IEnumerable<ArticleDto>>(results)
            };

            // Flip the following status of each article based on the current user on the request
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            foreach (var article in articlesViewModelList.Articles)
            {
                // Retrieve the corresponding article
                var mappedArticleEntity = results.FirstOrDefault(a => string.Equals(a.Title, article.Title, StringComparison.OrdinalIgnoreCase));

                // Set the following and favorited properties
                if (mappedArticleEntity != null)
                {
                    article.Author.Following = mappedArticleEntity.IsFollowingAuthor(currentUser);
                    article.Favorited = mappedArticleEntity.HasFavorited(currentUser);
                }
            }

            return articlesViewModelList;
        }
    }
}