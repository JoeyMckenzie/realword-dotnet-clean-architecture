namespace Conduit.Core.Articles.Commands.AddComment
{
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using MediatR;

    public class AddCommentCommand : IRequest<CommentViewModel>
    {
        public string Slug { get; set; }

        public AddCommentDto Comment { get; set; }
    }
}