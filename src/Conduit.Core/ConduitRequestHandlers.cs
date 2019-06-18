namespace Conduit.Core
{
    using System.Reflection;
    using Articles.Commands.CreateArticle;
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
                typeof(CreateArticleCommandHandler).Assembly
            };
        }
    }
}