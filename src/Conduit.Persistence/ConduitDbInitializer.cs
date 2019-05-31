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
            SeedConduitUsers(context, out string userId);
        }

        /// <summary>
        /// Seeds the user entity using Identity and returns the user's ID to be used in the test relations.
        /// </summary>
        /// <param name="context">Conduit database context</param>
        /// <param name="userId">Out parameter passed to subsequent seeder methods</param>
        private static void SeedConduitUsers(ConduitDbContext context, out string userId)
        {
            var testUser = new ConduitUser
            {
                Email = "joey.mckenzie27@gmail.com",
                UserName = "joey.mckenzie",
                Bio = "Lover of cheap and even cheaper wine.",
                Image = "https://joeymckenzie.azurewebsites.net/images/me.jpg",
            };

            testUser.PasswordHash = new PasswordHasher<ConduitUser>()
                .HashPassword(testUser, "password");

            context.Users.Add(testUser);
            context.SaveChanges();

            userId = testUser.Id;
        }
    }
}