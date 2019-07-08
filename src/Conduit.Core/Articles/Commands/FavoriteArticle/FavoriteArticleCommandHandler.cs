namespace Conduit.Core.Articles.Commands.FavoriteArticle
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.ViewModels;
    using Infrastructure;
    using MediatR;

    public class FavoriteArticleCommandHandler : IRequestHandler<FavoriteArticleCommand, ArticleViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IConduitDbContext _context;
        private readonly IMapper _mapper;

        public FavoriteArticleCommandHandler(ICurrentUserContext currentUserContext, IConduitDbContext context, IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public Task<ArticleViewModel> Handle(FavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}