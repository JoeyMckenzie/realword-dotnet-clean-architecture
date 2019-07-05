namespace Conduit.Core.Articles.Queries.GetArticle
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.ViewModels;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetArticleQueryHandler : IRequestHandler<GetArticleQuery, ArticleViewModel>
    {
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetArticleQueryHandler(ConduitDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ArticleViewModel> Handle(GetArticleQuery request, CancellationToken cancellationToken)
        {
            var articleFromSlug = await _context.Articles.FirstOrDefaultAsync();
            throw new System.NotImplementedException();
        }
    }
}