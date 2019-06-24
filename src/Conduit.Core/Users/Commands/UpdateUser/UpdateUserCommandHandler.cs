namespace Conduit.Core.Users.Commands.UpdateUser
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Persistence;
    using Persistence.Infrastructure;
    using Shared.Extensions;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            ICurrentUserContext currentUserContext,
            IMapper mapper,
            UserManager<ConduitUser> userManager,
            ConduitDbContext context)
        {
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _userManager = userManager;
            _context = context;
        }

        public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            // Invalidate user request that contain empty updates for username or email
            if (request.User.Email.IsValidEmptyString() || request.User.Username.IsValidEmptyString())
            {
                throw new ConduitApiException("Requests to update username, or email, must not be empty", HttpStatusCode.BadRequest);
            }

            var currentUser = await _currentUserContext.GetCurrentUserContext();

            // Validate the email is not in use if it has changed
            if (IsRequestPropertyAvailableForUpdate(request.User.Email, currentUser.Email))
            {
                var priorExistingEmail = await _userManager.FindByEmailAsync(request.User.Email);
                if (priorExistingEmail != null)
                {
                    throw new ConduitApiException($"Email {request.User.Email} is already in use", HttpStatusCode.BadRequest);
                }
            }

            // Validate the username is not in use if it has changed
            if (IsRequestPropertyAvailableForUpdate(request.User.Username, currentUser.UserName))
            {
                var priorExistingUsername = await _userManager.FindByNameAsync(request.User.Username);
                if (priorExistingUsername != null)
                {
                    throw new ConduitApiException($"Username {request.User.Username} is already in use", HttpStatusCode.BadRequest);
                }
            }

            // Update the fields
            currentUser.Email = request.User.Email ?? currentUser.Email;
            currentUser.UserName = request.User.Username ?? currentUser.Email;
            currentUser.Bio = request.User.Bio ?? currentUser.Email;
            currentUser.Image = request.User.Image ?? currentUser.Email;
            await _userManager.UpdateAsync(currentUser);

            // Map the response
            var userViewModel = new UserViewModel
            {
                User = _mapper.Map<UserDto>(currentUser)
            };
            userViewModel.User.Token = _currentUserContext.GetCurrentUserToken();

            await _context.AddActivityAndSaveChangesAsync(
                ActivityType.UserUpdated,
                TransactionType.ConduitUser,
                currentUser.Id,
                cancellationToken);

            return userViewModel;
        }

        private static bool IsRequestPropertyAvailableForUpdate(string requestProperty, string currentProperty)
        {
            return !string.IsNullOrWhiteSpace(requestProperty) &&
                   !string.Equals(requestProperty, currentProperty, StringComparison.OrdinalIgnoreCase);
        }
    }
}