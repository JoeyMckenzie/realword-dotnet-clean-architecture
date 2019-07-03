namespace Conduit.Core.Extensions
{
    using System.Linq;
    using Domain.Entities;

    public static class ArticleExtensions
    {
        public static bool IsFollowingAuthor(this Article article, ConduitUser user)
        {
            return article.Author.Followers.Any(f => f.UserFollower == user);
        }

        public static bool HasFavorited(this Article article, ConduitUser user)
        {
            return article.Favorites.Any(f => f.User == user);
        }
    }
}