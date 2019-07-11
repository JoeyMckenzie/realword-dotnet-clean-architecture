namespace Conduit.Core.Tags.Queries.GetTags
{
    using Domain.ViewModels;
    using MediatR;

    public class GetTagsQuery : IRequest<TagViewModel>
    {
    }
}