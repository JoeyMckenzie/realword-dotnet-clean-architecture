namespace Conduit.Core.Profiles.Commands.UnfollowUser
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Identity;

    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, ProfileViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly IConduitDbContext _context;
        private readonly IMapper _mapper;

        public UnfollowUserCommandHandler(
            ICurrentUserContext currentUserContext,
            UserManager<ConduitUser> userManager,
            IConduitDbContext context,
            IMapper mapper)
        {
            _currentUserContext = currentUserContext;
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfileViewModel> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the user to follow
            var userToUnfollow = await _userManager.FindByNameAsync(request.Username);
            if (userToUnfollow == null)
            {
                throw new ConduitApiException($"User [{request.Username}] was not found", HttpStatusCode.NotFound);
            }

            // Check for previously existing follows
            var requestingUser = await _currentUserContext.GetCurrentUserContext();
            if (requestingUser == userToUnfollow)
            {
                throw new ConduitApiException($"A user cannot unfollow themselves", HttpStatusCode.BadRequest);
            }

            var existingUserFollow = userToUnfollow.Followers
                .FirstOrDefault(u => u.UserFollower == requestingUser);

            if (existingUserFollow != null)
            {
                _context.UserFollows.Remove(existingUserFollow);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var profileViewModel = new ProfileViewModel
            {
                Profile = _mapper.Map<ProfileDto>(userToUnfollow)
            };
            profileViewModel.Profile.Following = false;

            return profileViewModel;
        }
    }
}