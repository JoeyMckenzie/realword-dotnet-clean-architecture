namespace Conduit.Api.Controllers
{
    using System.Threading.Tasks;
    using Core.Profiles.Queries.GetProfile;
    using Domain.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Authorize]
    public class ProfilesController : ConduitControllerBase
    {
        private readonly ILogger<ProfilesController> _logger;

        public ProfilesController(ILogger<ProfilesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(typeof(ProfileViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        public async Task<ProfileViewModel> GetProfileFromUsername(string username)
        {
            _logger.LogInformation($"Retrieve profile for user [{username}]");
            return await Mediator.Send(new GetProfileQuery(username));
        }
    }
}