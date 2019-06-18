namespace Conduit.Core.Articles.Commands.CreateArticle
{
    using Domain.Dtos;
    using Domain.ViewModels;
    using MediatR;

    public class CreateArticleCommand : IRequest<ArticleViewModel>
    {
        public ArticleDto Article { get; set; }
    }
}