namespace Conduit.Core.Tests.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shouldly;
    using Xunit;

    [Collection("ConduitCollectionFixture")]
    public class LoginUserCommandHandlerTest : TestFixture
    {
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandlerTest()
        {
            _logger = NullLogger<LoginUserCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUserExists_ReturnsProperUserViewModelResponse()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "test.user@gmail.com",
                    Password = "#passwordTwo1!"
                }
            };

            // Act
            var command = new LoginUserCommandHandler(UserManager, Context, TokenService, _logger, Mapper);
            var result = await command.Handle(userLoginCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.User.Email.ShouldBe(userLoginCommand.User.Email);
            result.User.Username.ShouldBe("test.user");
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUserDoesNotExists_ThrowsApiExceptionForBadRequest()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "test.user1@gmail.com",
                    Password = "#passwordTwo1!"
                }
            };

            // Act
            var command = new LoginUserCommandHandler(UserManager, Context, TokenService, _logger, Mapper);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(userLoginCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenThePasswordDoesNotMatch_ThrowsApiExceptionForBadRequest()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "test.user@gmail.com",
                    Password = "wrongPassword"
                }
            };

            // Act
            var command = new LoginUserCommandHandler(UserManager, Context, TokenService, _logger, Mapper);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(userLoginCommand, CancellationToken.None);
            });
        }
    }
}