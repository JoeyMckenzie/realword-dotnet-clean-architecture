namespace Conduit.Core.Articles.Commands.FavoriteArticle
{
    using Domain.ViewModels;
    using MediatR;

    public class FavoriteArticleCommand : IRequest<ArticleViewModel>
    {
        public FavoriteArticleCommand(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}