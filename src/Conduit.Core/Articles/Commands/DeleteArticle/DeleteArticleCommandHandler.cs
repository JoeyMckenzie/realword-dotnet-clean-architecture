namespace Conduit.Core.Articles.Commands.DeleteArticle
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Persistence;

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
            var articleToDelete = await _context.Articles
                .Where(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync(a => string.Equals(a.AuthorId, currentUser.Id, StringComparison.OrdinalIgnoreCase), cancellationToken);

            if (articleToDelete == null)
            {
                throw new ConduitApiException($"Article [{request.Slug}] was not found by user [{currentUser.Email}]", HttpStatusCode.BadRequest);
            }

            _context.Remove(articleToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Article [{request.Slug}] successfully removed");

            return Unit.Value;
        }
    }
}