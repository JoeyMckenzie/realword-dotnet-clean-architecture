namespace Conduit.Api.Extensions
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Infrastructure;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
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
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Conduit:JwtSecret"]));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Conduit:Issuer"],
                ValidAudience = configuration["Conduit:Audience"],
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };

            services.Configure<JwtOptionsConfiguration>(options =>
            {
                options.Issuer = configuration["Conduit:Issuer"];
                options.Audience = configuration["Conduit:Audience"];
                options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.ClaimsIssuer = configuration["Conduit:Issuer"];
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = tokenValidationParameters;
                    jwtBearerOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0].Split(" ")[1];
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
        }
    }
}