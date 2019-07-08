namespace Conduit.Core.Users.Commands.LoginUser
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain.Dtos.Users;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Extensions;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserViewModel>
    {
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ILogger<LoginUserCommandHandler> _logger;
        private readonly ITokenService _tokenService;
        private readonly IConduitDbContext _context;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(
            UserManager<ConduitUser> userManager,
            IConduitDbContext context,
            ITokenService tokenService,
            ILogger<LoginUserCommandHandler> logger,
            IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task<UserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.User.Email.ToUpperInvariant());
            if (existingUser == null)
            {
                throw new ConduitApiException($"No user found with email [{request.User.Email}]", HttpStatusCode.NotFound);
            }

            // Validate the users credentials
            var existingUserPasswordMatch = await _userManager.CheckPasswordAsync(existingUser, request.User.Password);
            if (!existingUserPasswordMatch)
            {
                throw new ConduitApiException($"Incorrect password attempting to login for [{existingUser.Id}] ({existingUser.Email})", HttpStatusCode.BadRequest);
            }

            // Map the user and generate the token
            var token = _tokenService.CreateToken(existingUser);
            var userViewModel = new UserViewModel
            {
                User = _mapper.Map<UserDto>(existingUser)
            };
            userViewModel.User.Token = token;

            _logger.LogInformation($"Login successful for user [{existingUser.Id}] ({existingUser.UserName})");
            await _context.AddActivityAndSaveChangesAsync(ActivityType.Login, TransactionType.ConduitUser, existingUser.Id, cancellationToken);
            return userViewModel;
        }
    }
}