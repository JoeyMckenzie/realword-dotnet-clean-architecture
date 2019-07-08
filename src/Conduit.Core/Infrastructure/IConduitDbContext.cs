namespace Conduit.Core.Infrastructure
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public interface IConduitDbContext
    {
        DbSet<ActivityLog> ActivityLogs { get; set; }

        DbSet<Article> Articles { get; set; }

        DbSet<Comment> Comments { get; set; }

        DbSet<Tag> Tags { get; set; }

        DbSet<ArticleTag> ArticleTags { get; set; }

        DbSet<Favorite> Favorites { get; set; }

        DbSet<UserFollow> UserFollows { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}