namespace Conduit.Core.Articles.Commands.AddComment
{
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using MediatR;

    public class AddCommentCommand : IRequest<CommentViewModel>
    {
        public AddCommentCommand(string slug, AddCommentDto comment)
        {
            Slug = slug;
            Comment = comment;
        }

        public string Slug { get; }

        public AddCommentDto Comment { get; }
    }
}