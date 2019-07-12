namespace Conduit.Core.Tags.Queries.GetTags
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, TagViewModelList>
    {
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TagViewModelList> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all tags from the database
            var tags = await _context.Tags.ToListAsync(cancellationToken);

            if (tags == null)
            {
                return new TagViewModelList();
            }

            // Map the tags to the view model
            var tagViewModel = new TagViewModelList();
            foreach (var tag in tags)
            {
                tagViewModel.Tags.Add(tag.Description);
            }

            return tagViewModel;
        }
    }
}