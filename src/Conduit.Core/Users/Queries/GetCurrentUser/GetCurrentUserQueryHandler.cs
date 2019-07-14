namespace Conduit.Core.Users.Queries.GetCurrentUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Users;
    using Domain.Entities;
    using Domain.ViewModels;
    using Infrastructure;
    using MediatR;
    using Persistence;
    using Persistence.Infrastructure;

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetCurrentUserQueryHandler(ICurrentUserContext currentUserContext, IMapper mapper, ConduitDbContext context)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserViewModel> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            // Retrieve the current user
            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Map to the user view model and log activity
            var userViewModel = new UserViewModel
            {
                User = _mapper.Map<UserDto>(currentUser)
            };
            await _context.AddActivityAndSaveChangesAsync(ActivityType.UserRetrieved, TransactionType.ConduitUser, currentUser.Id, cancellationToken);

            return userViewModel;
        }
    }
}