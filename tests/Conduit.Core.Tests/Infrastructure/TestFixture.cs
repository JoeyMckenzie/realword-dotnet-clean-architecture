namespace Conduit.Core.Tests.Infrastructure
{
    using System;
    using AutoMapper;
    using Core.Infrastructure;
    using Domain.Entities;
    using Factories;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Persistence;
    using Shared;

    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            // Configure services
            var services = new ServiceCollection();
            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ConduitDbContext>(options => options.UseInMemoryDatabase($"{Guid.NewGuid().ToString()}.db"));
            services.AddIdentity<ConduitUser, IdentityRole>()
                .AddEntityFrameworkStores<ConduitDbContext>();

            // Configure HTTP context for authentication
            var context = new DefaultHttpContext();
            context.Features.Set<IHttpAuthenticationFeature>(new HttpAuthenticationFeature());
            services.AddSingleton<IHttpContextAccessor>(_ => new HttpContextAccessor
            {
                HttpContext = context
            });

            // Configure current user accessor as a provider
            var serviceProvider = services.BuildServiceProvider();

            // Initialize the database with seed data and context accessors services
            var databaseContext = serviceProvider.GetRequiredService<ConduitDbContext>();
            ConduitDbInitializer.Initialize(databaseContext);

            // Create the services from configured providers
            Mapper = AutoMapperFactory.Create();
            MachineDateTime = new DateTimeTest();
            TokenService = new TokenServiceTest();
            Context = databaseContext;
            UserManager = serviceProvider.GetRequiredService<UserManager<ConduitUser>>();
            CurrentUserContext = new CurrentUserContextTest(UserManager);
        }

        protected ConduitDbContext Context { get; }

        protected UserManager<ConduitUser> UserManager { get; }

        protected IMapper Mapper { get; }

        protected ITokenService TokenService { get; }

        protected ICurrentUserContext CurrentUserContext { get; }

        private IDateTime MachineDateTime { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ConduitDbContextTestFactory.Destroy(Context);
            }
        }
    }
}