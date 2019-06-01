namespace Conduit.Api.Middleware
{
    using Microsoft.AspNetCore.Builder;

    public static class ConduitErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseConduitErrorHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ConduitErrorHandlingMiddleware>();
        }
    }
}