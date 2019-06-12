namespace Conduit.Core.Infrastructure
{
    using Domain.Entities;

    public interface ITokenService
    {
        string CreateToken(ConduitUser user);
    }
}