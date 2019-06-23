namespace Conduit.Core.Articles.Commands.DeleteArticle
{
    using MediatR;

    public class DeleteArticleCommand : IRequest
    {
        public DeleteArticleCommand(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}