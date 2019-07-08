namespace Conduit.Core.Profiles.Commands.FollowUser
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
    using Shared;

    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, ProfileViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly IConduitDbContext _context;
        private readonly IDateTime _dateTime;
        private readonly IMapper _mapper;

        public FollowUserCommandHandler(
            ICurrentUserContext currentUserContext,
            IConduitDbContext context,
            IMapper mapper,
            UserManager<ConduitUser> userManager,
            IDateTime dateTime)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _dateTime = dateTime;
        }

        public async Task<ProfileViewModel> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the user to follow
            var userToFollow = await _userManager.FindByNameAsync(request.Username);
            if (userToFollow == null)
            {
                throw new ConduitApiException($"User [{request.Username}] was not found", HttpStatusCode.NotFound);
            }

            // Check for previously existing follows
            var requestingUserFollower = await _currentUserContext.GetCurrentUserContext();
            if (requestingUserFollower == userToFollow)
            {
                throw new ConduitApiException("A user cannot follow themselves", HttpStatusCode.BadRequest);
            }

            var existingFollow = userToFollow.Followers
                .FirstOrDefault(u => u.UserFollower == requestingUserFollower);

            if (existingFollow == null)
            {
                // Create the new user follow between the follower and followee
                var newUserFollow = new UserFollow
                {
                    CreatedAt = _dateTime.Now,
                    UpdatedAt = _dateTime.Now,
                    UserFollower = requestingUserFollower,
                    UserFollowing = userToFollow
                };

                userToFollow.Followers.Add(newUserFollow);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var profileViewModel = new ProfileViewModel
            {
                Profile = _mapper.Map<ProfileDto>(userToFollow)
            };
            profileViewModel.Profile.Following = true;

            return profileViewModel;
        }
    }
}