namespace Conduit.Core.Articles.Commands.DeleteComment
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

    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IConduitDbContext _context;

        public DeleteCommentCommandHandler(IConduitDbContext context, ICurrentUserContext currentUserContext)
        {
            _context = context;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the article
            var article = await _context.Articles
                .Include(a => a.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(a => string.Equals(a.Slug, request.Slug, StringComparison.OrdinalIgnoreCase), cancellationToken);

            // Invalidate the request if the article is not found
            if (article == null)
            {
                throw new ConduitApiException($"Article [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Retrieve the comment
            var commentToDelete = article.Comments.FirstOrDefault(c => c.Id == request.Id);

            // Invalidate the request if the comment is not found
            if (commentToDelete == null)
            {
                throw new ConduitApiException($"Comment with ID [{request.Id}] was not found", HttpStatusCode.NotFound);
            }

            // Validate the request if the requester does not own the comment
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            if (currentUser != commentToDelete.User)
            {
                throw new ConduitApiException($"You do not own this comment and may not delete it", HttpStatusCode.Forbidden);
            }

            // Delete the comment
            _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}