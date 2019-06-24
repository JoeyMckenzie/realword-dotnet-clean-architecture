namespace Conduit.Core.Articles.Commands.DeleteArticle
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using Persistence.Infrastructure;

    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly ILogger<DeleteArticleCommandHandler> _logger;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;

        public DeleteArticleCommandHandler(ILogger<DeleteArticleCommandHandler> logger, ConduitDbContext context, ICurrentUserContext currentUserContext)
        {
            _logger = logger;
            _currentUserContext = currentUserContext;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            // Validate the user on the user on the request is the owner of the article
            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Retrieve the article, invalidate the request if none is found
            var articleToDelete = await _context.Articles.Where(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase)).ToListAsync(cancellationToken);
            if (!articleToDelete.Any())
            {
                throw new ConduitApiException($"Article [{request.Slug}] was not", HttpStatusCode.NotFound);
            }

            // Invalidate the request if the author is not found on any of the articles
            var authorOwnedArticleToDelete = articleToDelete.FirstOrDefault(a => string.Equals(a.AuthorId, currentUser.Id, StringComparison.OrdinalIgnoreCase));
            if (authorOwnedArticleToDelete == null)
            {
                throw new ConduitApiException($"Article [{request.Slug}] is not owned by author [{currentUser.Email}] and may not be deleted", HttpStatusCode.Unauthorized);
            }

            await _context.AddActivityAsync(
                ActivityType.ArticleDelete,
                TransactionType.Article,
                authorOwnedArticleToDelete.Title);
            _context.Remove(authorOwnedArticleToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Article [{request.Slug}] successfully removed");

            return Unit.Value;
        }
    }
}