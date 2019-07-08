namespace Conduit.Persistence
{
    using Core.Infrastructure;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ConduitDbContext : IdentityDbContext<ConduitUser>, IConduitDbContext
    {
        public ConduitDbContext(DbContextOptions<ConduitDbContext> options)
            : base(options)
        {
        }

        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ArticleTag> ArticleTags { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<UserFollow> UserFollows { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ConduitDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}