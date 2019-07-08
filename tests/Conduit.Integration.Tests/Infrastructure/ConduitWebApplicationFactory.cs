namespace Conduit.Integration.Tests.Infrastructure
{
    using System;
    using Core;
    using Core.Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;

    /// <summary>
    /// Integration testing harness for controllers tests.
    /// <see cref="https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2"/>
    /// </summary>
    /// <typeparam name="TStartup">Startup configuration for API</typeparam>
    public class ConduitWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<IConduitDbContext, ConduitDbContext>(options =>
                {
                    options.UseInMemoryDatabase($"{Guid.NewGuid().ToString()}.db");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var provider = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using (var scope = provider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var conduitDbContext = scopedServices.GetRequiredService<IConduitDbContext>();
                    var implementedContext = (ConduitDbContext)conduitDbContext;

                    // Ensure the database is created.
                    implementedContext.Database.EnsureCreated();

                    // Seed the database with test data.
                    ConduitDbInitializer.Initialize(implementedContext);
                }
            });
        }
    }
}