namespace Conduit.Core.Articles.Commands.AddComment
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Comments;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Persistence;
    using Persistence.Infrastructure;
    using Shared;

    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, CommentViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public AddCommentCommandHandler(ICurrentUserContext currentUserContext, ConduitDbContext context, IMapper mapper, IDateTime dateTime)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        public async Task<CommentViewModel> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the article
            var articleFromSlug = await _context.FirstArticleOrDefaultWithRelatedEntities(request.Slug, cancellationToken);

            // Invalid the request if the article was not found
            if (articleFromSlug == null)
            {
                throw new ConduitApiException($"Article with slug [{request.Slug}] was not found", HttpStatusCode.NotFound);
            }

            // Retrieve the current user on the request
            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Create the comment
            var newComment = new Comment
            {
                Article = articleFromSlug,
                Body = request.Comment.Body,
                User = currentUser,
                CreatedAt = _dateTime.Now,
                UpdatedAt = _dateTime.Now
            };

            // Add the comment to the database and link it to its associated article
            _context.Comments.Add(newComment);
            articleFromSlug.Comments.Add(newComment);
            await _context.SaveChangesAsync(cancellationToken);

            // Map the newly created comment
            var commentViewModel = new CommentViewModel
            {
                Comment = _mapper.Map<CommentDto>(newComment)
            };

            return commentViewModel;
        }
    }
}