namespace Conduit.Core.Articles.Queries.GetCommentsFromArticle
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using Exceptions;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;
    using Persistence.Infrastructure;

    public class ArticleCommentsQueryHandler : IRequestHandler<ArticleCommentsQuery, CommentViewModelList>
    {
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public ArticleCommentsQueryHandler(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentViewModelList> Handle(ArticleCommentsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the article from the request slug
            var articleFromSlug = await _context.Articles
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase), cancellationToken);

            // Invalidate the request if no article is found
            if (articleFromSlug == null)
            {
                throw new ConduitApiException($"Article with slug [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Map and return the results
            var commentViewModelList = new CommentViewModelList
            {
                Comments = _mapper.Map<IEnumerable<CommentDto>>(articleFromSlug.Comments)
            };

            return commentViewModelList;
        }
    }
}