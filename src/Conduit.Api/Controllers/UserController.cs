namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Users.Commands.UpdateUser;
    using Core.Users.Queries.GetCurrentUser;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class UserController : ConduitControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<UserViewModel> RetrieveCurrentUser()
        {
            _logger.LogInformation("Retrieving current user");
            return await Mediator.Send(new GetCurrentUserQuery());
        }

        [HttpPut]
        public async Task<UserViewModel> UpdateUser([FromBody] UpdateUserCommand command)
        {
            _logger.LogInformation($"Update user [{HttpContext.User.Identity?.Name}]");
            return await Mediator.Send(command);
        }
    }
}