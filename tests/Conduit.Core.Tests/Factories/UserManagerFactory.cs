namespace Conduit.Core.Tests.Factories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Extensions.Options;
    using Moq;

    public static class UserManagerFactory
    {
        public static UserManager<ConduitUser> Create(ConduitUser user)
        {
            var userStoreMock = new Mock<IUserStore<ConduitUser>>();
            var userEmailStoreMock = new Mock<IUserEmailStore<ConduitUser>>();

            // Setup store mock calls
            userStoreMock.Setup(s => s.CreateAsync(user, CancellationToken.None))
                .Returns(Task.FromResult(IdentityResult.Success));
            userEmailStoreMock.Setup(s => s.FindByEmailAsync(user.Email, CancellationToken.None))
                .Returns((Task<ConduitUser>)null);

            var options = new Mock<IOptions<IdentityOptions>>();
            var identityOptions = new IdentityOptions
            {
                Lockout =
                {
                    AllowedForNewUsers = false
                }
            };
            options.Setup(o => o.Value).Returns(identityOptions);
            var userValidators = new List<IUserValidator<ConduitUser>>();
            var validator = new Mock<IUserValidator<ConduitUser>>();
            userValidators.Add(validator.Object);
            var passwordValidators = new List<PasswordValidator<ConduitUser>>
            {
                new PasswordValidator<ConduitUser>()
            };
            var userManager = new UserManager<ConduitUser>(
                userEmailStoreMock.Object,
                options.Object,
                new PasswordHasher<ConduitUser>(),
                userValidators,
                passwordValidators,
                new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(),
                null,
                NullLogger<UserManager<ConduitUser>>.Instance);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<ConduitUser>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }
    }
}