namespace Conduit.Core.Articles.Queries.GetArticles
{
    using Domain.ViewModels;
    using MediatR;

    public class GetArticlesQuery : IRequest<ArticleViewModelList>
    {
        public GetArticlesQuery(string tag, string author, string favorited, int? limit, int? offset)
        {
            Tag = tag;
            Author = author;
            Favorited = favorited;
            Limit = limit ?? 20;
            Offset = offset ?? 0;
        }

        public string Tag { get; }

        public string Author { get; }

        public string Favorited { get; }

        public int Limit { get; }

        public int Offset { get; }
    }
}