namespace Conduit.Core.Infrastructure
{
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ICurrentUserContext
    {
        Task<ConduitUser> GetCurrentUserContext();
    }
}