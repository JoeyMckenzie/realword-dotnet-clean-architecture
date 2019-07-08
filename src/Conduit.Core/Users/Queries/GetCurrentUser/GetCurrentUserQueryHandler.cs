namespace Conduit.Core.Users.Queries.GetCurrentUser
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Extensions;
    using Infrastructure;
    using MediatR;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(ICurrentUserContext currentUserContext, IMapper mapper, IConduitDbContext context)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserViewModel> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _currentUserContext.GetCurrentUserContext().Result;

            if (currentUser == null)
            {
                throw new ConduitApiException("Could not retrieve the current user", HttpStatusCode.BadRequest);
            }

            // Map to the user view model and log activity
            var userViewModel = _mapper.Map<UserViewModel>(currentUser);
            await _context.AddActivityAndSaveChangesAsync(ActivityType.UserRetrieved, TransactionType.ConduitUser, currentUser.Id, cancellationToken);

            return userViewModel;
        }
    }
}