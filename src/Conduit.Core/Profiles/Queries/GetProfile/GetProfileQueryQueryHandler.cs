namespace Conduit.Core.Profiles.Queries.GetProfile
{
    using System;
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
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public class GetProfileQueryQueryHandler : IRequestHandler<GetProfileQuery, ProfileViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public GetProfileQueryQueryHandler(IMapper mapper, ConduitDbContext context, ICurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProfileViewModel> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            // Search for users using the user manager
            var existingUser = await _context.Users
                .Where(u => string.Equals(u.UserName, request.Username, StringComparison.OrdinalIgnoreCase))
                .Include(u => u.Followers)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser == null)
            {
                throw new ConduitApiException($"User [{request.Username}] was not found", HttpStatusCode.NotFound);
            }

            // Map the user entity to the view model
            var profileViewModel = new ProfileViewModel
            {
                Profile = _mapper.Map<ProfileDto>(existingUser)
            };

            // Get user follows from the requester
            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Get all the user follows for the current user
            var userFollowForCurrentUser = existingUser.Followers
                .FirstOrDefault(uf => string.Equals(uf.UserFollowerId, currentUser.Id, StringComparison.OrdinalIgnoreCase));
            profileViewModel.Profile.Following = userFollowForCurrentUser != null;

            return profileViewModel;
        }
    }
}