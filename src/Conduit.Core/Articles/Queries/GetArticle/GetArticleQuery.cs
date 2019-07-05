namespace Conduit.Core.Articles.Queries.GetArticle
{
    using Domain.ViewModels;
    using MediatR;

    public class GetArticleQuery : IRequest<ArticleViewModel>
    {
        public GetArticleQuery(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}