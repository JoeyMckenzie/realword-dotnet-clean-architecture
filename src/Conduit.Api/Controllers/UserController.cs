namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
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
        public Task<UserViewModel> RetrieveCurrentUser()
        {
            _logger.LogInformation("Retrieving current user");
            return Mediator.Send(new GetCurrentUserQuery());
        }
    }
}