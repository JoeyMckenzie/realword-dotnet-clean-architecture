namespace Conduit.Core.Users.Commands.CreateUser
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

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserViewModel>
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly UserManager<ConduitUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ConduitDbContext _context;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(
            ILogger<CreateUserCommandHandler> logger,
            UserManager<ConduitUser> userManager,
            ConduitDbContext context,
            IMapper mapper,
            ITokenService tokenService)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<UserViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate the username is not in use
            var existingUserByUserName = await _userManager.FindByNameAsync(request.User.Username);
            if (existingUserByUserName != null)
            {
                throw new ConduitApiException($"Username {request.User.Username} is already in use", HttpStatusCode.BadRequest);
            }

            // Validate the email is not in use
            var existingUserByEmail = await _userManager.FindByEmailAsync(request.User.Email);
            if (existingUserByEmail != null)
            {
                throw new ConduitApiException($"Email {request.User.Email} is already in use", HttpStatusCode.BadRequest);
            }

            // Instantiate and attempt to create the user
            var newUser = new ConduitUser
            {
                UserName = request.User.Username,
                Email = request.User.Email,
            };
            var createUserResult = await _userManager.CreateAsync(newUser, request.User.Password);

            // Instantiate the creation exception and errors, if necessary
            if (!createUserResult.Succeeded)
            {
                var exception = new ConduitApiException($"Could not create user with [{request.User.Username}]", HttpStatusCode.BadRequest);

                foreach (var error in createUserResult.Errors)
                {
                    var conduitError = new ConduitApiError(error.Code, error.Description);
                    exception.ApiErrors.Add(conduitError);
                }

                throw exception;
            }

            // Generate the token and map the user
            var token = _tokenService.CreateToken(newUser);
            var userViewModel = new UserViewModel
            {
                User = _mapper.Map<UserDto>(newUser)
            };
            userViewModel.User.Token = token;

            await _context.AddActivityAndSaveChangesAsync(ActivityType.UserCreated, TransactionType.ConduitUser, newUser.Id, cancellationToken);
            _logger.LogInformation($"{request.User.Username}] created successfully");
            return userViewModel;
        }
    }
}