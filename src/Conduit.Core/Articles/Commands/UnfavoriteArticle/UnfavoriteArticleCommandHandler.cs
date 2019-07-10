namespace Conduit.Core.Articles.Commands.UnfavoriteArticle
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.ViewModels;
    using Infrastructure;
    using MediatR;
    using Persistence;

    public class UnfavoriteArticleCommandHandler : IRequestHandler<UnfavoriteArticleCommand, ArticleViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public UnfavoriteArticleCommandHandler(ICurrentUserContext currentUserContext, ConduitDbContext context, IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public Task<ArticleViewModel> Handle(UnfavoriteArticleCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}