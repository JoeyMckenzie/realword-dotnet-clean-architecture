namespace Conduit.Core.Tests.Infrastructure
{
    using System.Threading.Tasks;
    using Core.Infrastructure;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    public class CurrentUserContextTest : ICurrentUserContext
    {
        private readonly UserManager<ConduitUser> _userManager;

        public CurrentUserContextTest(UserManager<ConduitUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConduitUser> GetCurrentUserContext()
        {
            return await _userManager.FindByEmailAsync(TestConstants.TestUserEmail);
        }

        public string GetCurrentUserToken()
        {
            return TestConstants.TokenStringMock;
        }
    }
}