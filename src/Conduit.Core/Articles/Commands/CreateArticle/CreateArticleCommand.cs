namespace Conduit.Core.Articles.Commands.CreateArticle
{
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using MediatR;

    public class CreateArticleCommand : IRequest<ArticleViewModel>
    {
        public CreateArticleDto Article { get; set; }
    }
}