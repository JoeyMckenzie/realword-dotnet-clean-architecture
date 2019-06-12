namespace Conduit.Core.Users.Commands.UpdateUser
{
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
    using Microsoft.Extensions.Logging;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserViewModel>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(
            ICurrentUserContext currentUserContext,
            IMapper mapper,
            ILogger<UpdateUserCommandHandler> logger,
            ITokenService tokenService,
            UserManager<ConduitUser> userManager)
        {
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _logger = logger;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<UserViewModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _currentUserContext.GetCurrentUserContext();
            if (currentUser == null)
            {
                throw new ConduitApiException($"Could update user information for request email {request.User}", HttpStatusCode.BadRequest);
            }

            // Update the fields
            currentUser.Email = request.User.Email ?? request.User.Email;
            currentUser.UserName = request.User.Username ?? request.User.Username;
            currentUser.Bio = request.User.Bio ?? request.User.Bio;
            currentUser.Image = request.User.Image ?? request.User.Image;
            await _userManager.UpdateAsync(currentUser);

            // Map the response
            var userViewModel = new UserViewModel
            {
                User = _mapper.Map<UserDto>(currentUser)
            };
            userViewModel.User.Token = await _currentUserContext.GetCurrentUserToken();

            return userViewModel;
        }
    }
}