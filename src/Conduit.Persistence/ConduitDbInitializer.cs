namespace Conduit.Persistence
{
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;

    public class ConduitDbInitializer
    {
        public static void Initialize(ConduitDbContext context)
        {
            SeedEntitiesInDatabase(context);
        }

        private static void SeedEntitiesInDatabase(ConduitDbContext context)
        {
            SeedConduitUsers(context, out string userId, out string testUserId);
            SeedUserFollows(context, testUserId, userId);
            SeedUserFollows(context, userId, testUserId);
            SeedArticles(context, userId, out int articleId);
            SeedFavorites(context, articleId, testUserId);
            SeedTags(context, articleId);
            SeedComments(context, userId, articleId);
        }

        /// <summary>
        /// Seeds the user entity using Identity and returns the user's ID to be used in the test relations.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="userId">Out parameter passed to subsequent seeder methods</param>
        private static void SeedConduitUsers(ConduitDbContext context, out string userId, out string testUserId)
        {
            var testUser1 = new ConduitUser
            {
                Email = "joey.mckenzie@gmail.com",
                NormalizedEmail = "joey.mckenzie@gmail.com".ToUpperInvariant(),
                UserName = "joey.mckenzie",
                NormalizedUserName = "joey.mckenzie".ToUpperInvariant(),
                Bio = "Lover of cheap and even cheaper wine.",
                Image = "https://joeymckenzie.azurewebsites.net/images/me.jpg",
                SecurityStamp = "someRandomSecurityStamp"
            };

            testUser1.PasswordHash = new PasswordHasher<ConduitUser>()
                .HashPassword(testUser1, "#password1!");

            var testUser2 = new ConduitUser
            {
                Email = "test.user@gmail.com",
                NormalizedEmail = "test.user@gmail.com".ToUpperInvariant(),
                UserName = "test.user",
                NormalizedUserName = "test.user".ToUpperInvariant(),
                Bio = "I AM NOT A ROBOT.",
                Image = "https://joeymckenzie.azurewebsites.net/images/me.jpg",
                SecurityStamp = "someRandomSecurityStamp"
            };

            testUser2.PasswordHash = new PasswordHasher<ConduitUser>()
                .HashPassword(testUser2, "#passwordTwo1!");

            context.Users.Add(testUser1);
            context.Users.Add(testUser2);
            context.SaveChanges();

            userId = testUser1.Id;
            testUserId = testUser2.Id;
        }

        /// <summary>
        /// Seeds an article entity and returns the article ID to be used for linking the seeded tags.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="userId">User ID associated with the article</param>
        /// <param name="articleId">Out Article ID passed to subsequent seeder methods</param>
        private static void SeedArticles(ConduitDbContext context, string userId, out int articleId)
        {
            var testArticle = new Article
            {
                Title = "How to train your dragon",
                Description = "Ever wonder how?",
                Body = "Very carefully.",
                Slug = "how-to-train-your-dragon",
                AuthorId = userId
            };

            context.Articles.Add(testArticle);
            context.SaveChanges();
            articleId = testArticle.Id;
        }

        /// <summary>
        /// Seeds tag and article tag entities in the database from the give article ID.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="articleId">Article ID passed from the article seed method</param>
        private static void SeedTags(ConduitDbContext context, int articleId)
        {
            var testTag1 = new Tag
            {
                Description = "dragons"
            };

            var testTag2 = new Tag
            {
                Description = "training"
            };

            context.Tags.Add(testTag1);
            context.Tags.Add(testTag2);
            context.SaveChanges();

            var testArticleTag1 = new ArticleTag
            {
                ArticleId = articleId,
                TagId = testTag1.Id
            };

            var testArticleTag2 = new ArticleTag
            {
                ArticleId = articleId,
                TagId = testTag2.Id
            };

            context.ArticleTags.Add(testArticleTag1);
            context.ArticleTags.Add(testArticleTag2);
            context.SaveChanges();
        }

        /// <summary>
        /// Seeds comment entities by a given user ID and article ID.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="userId">User ID passed from the seed user method</param>
        /// <param name="articleId">Article ID passed from the seed article method</param>
        private static void SeedComments(ConduitDbContext context, string userId, int articleId)
        {
            var comment = new Comment
            {
                Body = "Thank you so much!",
                UserId = userId,
                ArticleId = articleId
            };

            context.Comments.Add(comment);
            context.SaveChanges();
        }

        /// <summary>
        /// Seeds a user follower to a followee.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="followerId">The user awarding the follow</param>
        /// <param name="followeeId">The user receiving the follow</param>
        private static void SeedUserFollows(ConduitDbContext context, string followerId, string followeeId)
        {
            var userFollow = new UserFollow
            {
                UserFollowerId = followerId,
                UserFollowingId = followeeId
            };

            context.UserFollows.Add(userFollow);
            context.SaveChanges();
        }

        /// <summary>
        /// Seeds a user favorite for a given article.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="articleId">Article ID to favorite</param>
        /// <param name="userId">User ID attempting to favorite the article</param>
        private static void SeedFavorites(ConduitDbContext context, int articleId, string userId)
        {
            var favorite = new Favorite
            {
                ArticleId = articleId,
                UserId = userId
            };
            context.Favorites.Add(favorite);

            // Update the favorite count for the article and save the changes
            var article = context.Articles.Find(articleId);
            article.FavoritesCount++;
            context.Articles.Update(article);

            context.SaveChanges();
        }
    }
}