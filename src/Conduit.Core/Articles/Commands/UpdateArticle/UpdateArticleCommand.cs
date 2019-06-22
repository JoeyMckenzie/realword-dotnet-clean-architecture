namespace Conduit.Core.Articles.Commands.UpdateArticle
{
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using MediatR;

    public class UpdateArticleCommand : IRequest<ArticleViewModel>
    {
        public string Slug { get; set; }

        public UpdateArticleDto Article { get; set; }
    }
}