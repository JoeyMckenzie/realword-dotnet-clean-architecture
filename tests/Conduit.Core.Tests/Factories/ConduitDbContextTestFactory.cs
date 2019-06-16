namespace Conduit.Core.Tests.Factories
{
    using System.Linq;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Persistence;

    public static class ConduitDbContextTestFactory
    {
        public static ConduitDbContext Create(out ConduitUser user)
        {
            var options = new DbContextOptionsBuilder<ConduitDbContext>()
                .UseInMemoryDatabase("Brewdude.Application.Tests.Db")
                .Options;

            var context = new ConduitDbContext(options);
            context.Database.EnsureCreated();
            ConduitDbInitializer.Initialize(context);
            user = context.Users.FirstOrDefault();

            return context;
        }

        public static void Destroy(ConduitDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}