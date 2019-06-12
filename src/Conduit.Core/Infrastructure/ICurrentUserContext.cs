namespace Conduit.Core.Infrastructure
{
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface ICurrentUserContext
    {
        Task<ConduitUser> GetCurrentUserContext();

        Task<string> GetCurrentUserToken();
    }
}