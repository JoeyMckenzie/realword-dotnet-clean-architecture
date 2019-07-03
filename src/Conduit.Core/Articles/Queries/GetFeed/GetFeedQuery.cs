namespace Conduit.Core.Articles.Queries.GetFeed
{
    using Domain.ViewModels;
    using MediatR;

    public class GetFeedQuery : IRequest<ArticleViewModelList>
    {
        public GetFeedQuery(int? limit, int? offset)
        {
            Limit = limit ?? 20;
            Offset = offset ?? 0;
        }

        public int Limit { get; }

        public int Offset { get; }
    }
}