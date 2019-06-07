namespace Conduit.Api
{
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Infrastructure;
    using Domain.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly UserManager<ConduitUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserContext(UserManager<ConduitUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<ConduitUser> GetCurrentUserContext()
        {
            var currentHttpContext = _contextAccessor.HttpContext;
            return await _userManager.GetUserAsync(currentHttpContext?.User);
        }
    }
}