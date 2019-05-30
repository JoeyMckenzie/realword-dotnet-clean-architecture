namespace Conduit.Persistence
{
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;

    public class ConduitDbContextFactory : DesignTimeDbContextFactoryBase<ConduitDbContext>
    {
        protected override ConduitDbContext CreateNewInstance(DbContextOptions<ConduitDbContext> options)
        {
            return new ConduitDbContext(options);
        }
    }
}