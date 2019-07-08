namespace Conduit.Core.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using Shared.Extensions;

    public static class DbContextExtensions
    {
        public static async Task AddActivityAsync(
            this IConduitDbContext dbContext,
            ActivityType activityType,
            TransactionType transactionType,
            string transactionId)
        {
            var activityLog = new ActivityLog
            {
                ActivityType = activityType.GetDescription(),
                TransactionId = transactionId,
                TransactionType = transactionType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await dbContext.ActivityLogs.AddAsync(activityLog);
        }

        public static async Task AddActivityAndSaveChangesAsync(
            this IConduitDbContext dbContext,
            ActivityType activityType,
            TransactionType transactionType,
            string transactionId,
            CancellationToken cancellationToken)
        {
            await AddActivityAsync(dbContext, activityType, transactionType, transactionId);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public static async Task<Article> FirstArticleOrDefaultWithRelatedEntities(this IConduitDbContext context, string slug, CancellationToken cancellationToken)
        {
            return await context.Articles
                .Include(a => a.Author)
                    .ThenInclude(au => au.Followers)
                .Include(a => a.ArticleTags)
                    .ThenInclude(at => at.Tag)
                .Include(a => a.Favorites)
                    .ThenInclude(f => f.User)
                .FirstOrDefaultAsync(a => string.Equals(a.Slug, slug, StringComparison.OrdinalIgnoreCase), cancellationToken);
        }
    }
}