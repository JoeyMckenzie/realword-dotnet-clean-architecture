namespace Conduit.Core
{
    using System.Reflection;
    using Articles.Commands.CreateArticle;
    using Articles.Commands.DeleteArticle;
    using Profiles.Commands.FollowUser;
    using Profiles.Commands.UnfollowUser;
    using Profiles.Queries.GetProfile;
    using Users.Commands.CreateUser;
    using Users.Commands.LoginUser;
    using Users.Commands.UpdateUser;
    using Users.Queries.GetCurrentUser;

    public static class ConduitRequestHandlers
    {
        public static Assembly[] GetRequestHandlerAssemblies()
        {
            return new[]
            {
                typeof(CreateUserCommandHandler).Assembly,
                typeof(LoginUserCommandHandler).Assembly,
                typeof(GetCurrentUserQueryHandler).Assembly,
                typeof(UpdateUserCommandHandler).Assembly,
                typeof(CreateArticleCommandHandler).Assembly,
                typeof(UpdateUserCommandHandler).Assembly,
                typeof(DeleteArticleCommand).Assembly,
                typeof(GetProfileQueryQueryHandler).Assembly,
                typeof(FollowUserCommandHandler).Assembly,
                typeof(UnfollowUserCommandHandler).Assembly
            };
        }
    }
}