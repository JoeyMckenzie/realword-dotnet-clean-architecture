namespace Conduit.Core.Articles.Commands.DeleteComment
{
    using MediatR;

    public class DeleteCommentCommand : IRequest
    {
        public DeleteCommentCommand(int id, string slug)
        {
            Id = id;
            Slug = slug;
        }

        public string Slug { get; }

        public int Id { get; }
    }
}