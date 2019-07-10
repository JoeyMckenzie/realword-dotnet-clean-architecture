namespace Conduit.Core.Articles.Commands.UnfavoriteArticle
{
    using Domain.ViewModels;
    using MediatR;

    public class UnfavoriteArticleCommand : IRequest<ArticleViewModel>
    {
        public UnfavoriteArticleCommand(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}