namespace Conduit.Core.Tests.Users
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Users.Commands.CreateUser;
    using Domain.Dtos;
    using Domain.Entities;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shouldly;
    using Xunit;

    [Collection("ConduitCollectionFixture")]
    public class CreateUserCommandHandlerTest : TestFixture
    {
        private readonly ILogger<CreateUserCommandHandler> _logger;

        public CreateUserCommandHandlerTest()
        {
            _logger = NullLogger<CreateUserCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenValidUserCommand_WhenTheUsernameAndEmailAreUnique_ShouldReturnSuccessfulUserViewModelResponse()
        {
            // Arrange
            var createUserCommand = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Username = "testUsername",
                    Email = "testEmail@gmail.com",
                    Password = "#testPassword1!"
                }
            };

            // Act
            var command = new CreateUserCommandHandler(_logger, UserManager, Context, Mapper, TokenService);
            var result = await command.Handle(createUserCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserViewModel>();
            result.User.ShouldNotBeNull();
            result.User.Username.ShouldBe(createUserCommand.User.Username);
            result.User.Email.ShouldBe(createUserCommand.User.Email);
            Context.Users.Single(u => u.UserName == createUserCommand.User.Username).ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenRequestContainingExistingUsername_WhenTheUserAlreadyExists_ThrowsExceptionWithBadRequest()
        {
            // Arrange, seed an existing user and new command
            var createUserCommand = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Username = "testUsername",
                    Email = "testEmail@gmail.com",
                    Password = "#testPassword1!"
                }
            };

            var existingUserWithSameUsername = new ConduitUser
            {
                Email = "testEmail1@gmail.com",
                NormalizedEmail = "testEmail1@gmail.com".ToUpperInvariant(),
                UserName = "testUsername",
                NormalizedUserName = "testUsername".ToUpperInvariant(),
                SecurityStamp = "someRandomSecurityStamp"
            };

            existingUserWithSameUsername.PasswordHash = new PasswordHasher<ConduitUser>()
                .HashPassword(existingUserWithSameUsername, "password");
            await UserManager.CreateAsync(existingUserWithSameUsername);

            // Act
            var command = new CreateUserCommandHandler(_logger, UserManager, Context, Mapper, TokenService);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(createUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenRequestContainingExistingEmail_WhenTheUserAlreadyExists_ThrowsExceptionWithBadRequest()
        {
            // Arrange, seed an existing user and new command
            var createUserCommand = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Username = "testUsername",
                    Email = "testEmail@gmail.com",
                    Password = "#testPassword1!"
                }
            };

            var existingUserWithSameUsername = new ConduitUser
            {
                Email = "testEmail@gmail.com",
                NormalizedEmail = "testEmail1@gmail.com".ToUpperInvariant(),
                UserName = "testUsername1",
                NormalizedUserName = "testUsername1".ToUpperInvariant(),
                SecurityStamp = "someRandomSecurityStamp"
            };

            existingUserWithSameUsername.PasswordHash = new PasswordHasher<ConduitUser>()
                .HashPassword(existingUserWithSameUsername, "password");
            await UserManager.CreateAsync(existingUserWithSameUsername);

            // Act
            var command = new CreateUserCommandHandler(_logger, UserManager, Context, Mapper, TokenService);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(createUserCommand, CancellationToken.None);
            });
        }
    }
}