namespace Conduit.Api
{
    using System.Reflection;
    using AutoMapper;
    using Core;
    using Core.Infrastructure;
    using Core.Users.Commands.CreateUser;
    using Domain.Entities;
    using Extensions;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using Infrastructure;
    using Infrastructure.Security;
    using MediatR;
    using MediatR.Pipeline;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;
    using Middleware;
    using Persistence;
    using Shared;
    using Shared.Constants;

    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add health checks
            services.AddHealthChecks()
                .AddSqlServer(
                    Configuration["CONNECTION_STRING"],
                    "SELECT 1;",
                    "Conduit Health Query",
                    HealthStatus.Degraded,
                    new[] { "ConduitDb" })
                .AddDbContextCheck<ConduitDbContext>(
                    "ConduitDbContextHealthCheck",
                    customTestQuery: async (context, token) => await context.ActivityLogs.AsNoTracking().ToListAsync(token) != null);

            // Add EF Core
            services.AddDbContext<ConduitDbContext>(options =>
                options.UseSqlServer(Configuration["CONNECTION_STRING"]));

            // Add Identity
            services.AddIdentity<ConduitUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ConduitDbContext>()
                .AddDefaultTokenProviders();

            // Add miscellaneous services
            services.AddAutoMapper(typeof(MappingProfile).GetTypeInfo().Assembly);
            services.AddTransient<IDateTime, MachineDateTime>();
            services.AddTransient<ICurrentUserContext, CurrentUserContext>();

            // Add MediatR pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddMediatR(ConduitRequestHandlers.GetRequestHandlerAssemblies());

            // Add swagger
            services.AddSwashbuckleSwagger();

            // Add authentication and the token service
            services.AddJwtAuthentication(_logger, Configuration);
            services.AddTransient<ITokenService>(_ => new TokenService(Configuration, new MachineDateTime()));

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>())
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateFormatHandling = ConduitConstants.ConduitJsonSerializerSettings.DateFormatHandling;
                });
            services.AddCors();

            // Override built in model state validation
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Configure cross-origin requests
            app.UseCors(options =>
            {
                options.WithMethods(HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete, HttpMethods.Options);
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
            });

            // Configure health checks
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                AllowCachingResponses = false
            });
            app.UseConduitErrorHandlingMiddleware();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    $"Conduit API version {ConduitConstants.ApiVersion}"));
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
