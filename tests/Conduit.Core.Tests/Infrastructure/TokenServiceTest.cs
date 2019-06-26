namespace Conduit.Core.Tests.Infrastructure
{
    using Core.Infrastructure;
    using Domain.Entities;

    public class TokenServiceTest : ITokenService
    {
        public string CreateToken(ConduitUser user)
        {
            return "aSecurityToken";
        }
    }
}