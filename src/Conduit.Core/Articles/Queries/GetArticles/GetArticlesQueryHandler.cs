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
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery, ArticleViewModelList>
    {
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetArticlesQueryHandler(
            UserManager<ConduitUser> userManager,
            ConduitDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModelList> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all articles from the database and include the corresponding
            var articles = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.Favorites)
                .AsQueryable();
            ConduitUser author = null;

            // Filter on author
            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                author = await _userManager.FindByNameAsync(request.Author);
                if (author != null)
                {
                    articles = articles.Where(a => a.Author == author);
                }
            }

            // Filter on tags
            if (!string.IsNullOrWhiteSpace(request.Tag))
            {
                var tag = await _context.Tags
                    .FirstOrDefaultAsync(t => string.Equals(t.Description, request.Tag, StringComparison.OrdinalIgnoreCase), cancellationToken);
                if (tag != null)
                {
                    articles = articles.Where(a => a.ArticleTags.Select(at => at.Tag).Contains(tag));
                }
            }

            // Filter on favorited
            if (!string.IsNullOrWhiteSpace(request.Favorited))
            {
                articles = articles.Where(a => a.Favorites.Select(f => f.User).Contains(author));
            }

            // Paginate and map the results
            var results = await articles
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(cancellationToken);

            var articlesViewModelList = new ArticleViewModelList
            {
                Articles = _mapper.Map<IEnumerable<ArticleDto>>(results)
            };

            return articlesViewModelList;
        }
    }
}