namespace Conduit.Api
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Persistence;

    public class Program
    {
        public static void Main(string[] args)
        {
            // CreateWebHostBuilder(args).Build().Run();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var host = CreateWebHostBuilder(args).Build();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (IsDatabaseRerollEnabled(args, environment))
            {
                // Seed database
                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<ConduitDbContext>();

                        // Drop the tables to recreate them with fresh data every server re-roll
                        Console.WriteLine("Initializing database contexts");
                        var timer = new Stopwatch();
                        timer.Start();
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        // Remove database seed for passing postman tests
                        // ConduitDbInitializer.Initialize(context);
                        timer.Stop();
                        Console.WriteLine($"Seeding databases, time to initialize {timer.ElapsedMilliseconds} ms");
                    }
                    catch (Exception e)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(e, "Could not seed database");
                    }
                }
            }

            try
            {
                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Host terminated unexpectedly. Reason: {e.Message}");
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Startup>();
                })
                .UseStartup<Startup>();

        private static bool IsDatabaseRerollEnabled(string[] args, string environment)
        {
            if (!string.IsNullOrWhiteSpace(environment) && environment.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                return args.Any(arg => arg == "reroll");
            }

            return false;
        }
    }
}
