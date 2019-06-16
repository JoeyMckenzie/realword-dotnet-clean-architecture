namespace Conduit.Api
{
    using System;
    using System.Threading.Tasks;
    using Core.Infrastructure;
    using Domain.Entities;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public string GetCurrentUserToken()
        {
            var token = string.Empty;
            var authorizationHeader = _contextAccessor.HttpContext.Request.Headers?["Authorization"];
            if (authorizationHeader.HasValue && authorizationHeader.ToString().StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
            {
                token = authorizationHeader.ToString().Split(' ')[1];
            }

            return token;
        }
    }
}