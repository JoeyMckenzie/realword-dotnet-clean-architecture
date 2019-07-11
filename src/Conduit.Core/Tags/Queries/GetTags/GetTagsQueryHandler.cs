namespace Conduit.Core.Tags.Queries.GetTags
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.ViewModels;
    using MediatR;
    using Persistence;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, TagViewModel>
    {
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<TagViewModel> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}