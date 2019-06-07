namespace Conduit.Core.Users.Commands.LoginUser
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
    using Persistence;
    using Persistence.Infrastructure;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserViewModel>
    {
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ILogger<LoginUserCommand> _logger;
        private readonly ITokenService _tokenService;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(
            UserManager<ConduitUser> userManager,
            ConduitDbContext context,
            ITokenService tokenService,
            ILogger<LoginUserCommand> logger,
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
            var existingUserVerifiedByPassword = await _userManager.CheckPasswordAsync(existingUser, request.User.Password);
            if (!existingUserVerifiedByPassword)
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