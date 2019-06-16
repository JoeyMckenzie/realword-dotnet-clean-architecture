namespace Conduit.Core.Tests.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Users.Commands.UpdateUser;
    using Domain.Dtos;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shouldly;
    using Xunit;

    [Collection("ConduitCollectionFixture")]
    public class UpdateUserCommandHandlerTest : TestFixture
    {
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandlerTest()
        {
            _logger = NullLogger<UpdateUserCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUserExists_ReturnsUpdateUserViewModelResponse()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Email = "myUpdatedEmail@gmail.com",
                    Username = "myUpdatedUsername",
                    Bio = "My updated bio",
                    Image = "Something super duper sexy"
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context);
            var result = await command.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.User.Email.ShouldBe(updateUserCommand.User.Email);
            result.User.Username.ShouldBe(updateUserCommand.User.Username);
            result.User.Bio.ShouldBe(updateUserCommand.User.Bio);
            result.User.Image.ShouldBe(updateUserCommand.User.Image);
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUpdatedUsernameAlreadyExists_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Username = "joey.mckenzie",
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(updateUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUpdatedEmailAlreadyExists_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Email = "joey.mckenzie@gmail.com",
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(updateUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUpdatedEmailIsEmpty_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Email = string.Empty,
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(updateUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUpdatedUsernameIsEmpty_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserDto
                {
                    Username = string.Empty,
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(updateUserCommand, CancellationToken.None);
            });
        }
    }
}