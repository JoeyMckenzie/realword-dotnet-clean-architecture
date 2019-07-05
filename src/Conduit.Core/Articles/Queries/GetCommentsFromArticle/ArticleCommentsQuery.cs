namespace Conduit.Core.Articles.Queries.GetCommentsFromArticle
{
    using Domain.ViewModels;
    using MediatR;

    public class ArticleCommentsQuery : IRequest<CommentViewModelList>
    {
        public ArticleCommentsQuery(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}