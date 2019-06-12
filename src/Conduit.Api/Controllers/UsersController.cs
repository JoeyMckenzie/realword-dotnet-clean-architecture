namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Users.Commands.CreateUser;
    using Core.Users.Commands.LoginUser;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class UsersController : ConduitControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<UserViewModel> Register([FromBody] CreateUserCommand command)
        {
            _logger.LogInformation($"Creating user for request [{command.User.Email}]");
            return await Mediator.Send(command);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        public async Task<UserViewModel> Login([FromBody] LoginUserCommand command)
        {
            _logger.LogInformation($"Attempting login request for user [{command.User.Email}]");
            return await Mediator.Send(command);
        }
    }
}