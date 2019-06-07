namespace Conduit.Api.Extensions
{
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using Persistence;
    using Swashbuckle.AspNetCore.Swagger;

    public static class StartupExtensions
    {
        public static void AddSwashbuckleSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Conduit API",
                    Version = "v1",
                    Description = "API endpoints for the Conduit application.",
                    Contact = new Contact
                    {
                        Name = "Joey Mckenzie",
                        Email = "joey.mckenzie27@gmail.com",
                        Url = "https://azurewebsites.joeymckenzie.com"
                    }
                });
            });
        }

        public static void AddJwtAuthentication(this IServiceCollection services, ILogger<Startup> logger, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userContext = context.HttpContext.RequestServices.GetRequiredService<ConduitDbContext>();
                            var userId = context.Principal?.Identity?.Name;

                            // No userId found on token
                            if (userId == null)
                            {
                                return Task.CompletedTask;
                            }

                            // Attempt to validate the user on the request
                            var user = userContext.Users.Find(userId);
                            if (user == null)
                            {
                                // Return unauthorized if user no longer exists
                                logger.LogError($"User with userId [{userId}] no longer exists");
                                context.Fail("User is unauthorized to make the request");
                            }
                            return Task.CompletedTask;
                        }
                    };
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["ConduitJwtSecret"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }
    }
}