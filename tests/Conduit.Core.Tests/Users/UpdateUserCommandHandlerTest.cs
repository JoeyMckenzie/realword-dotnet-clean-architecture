namespace Conduit.Core.Tests.Users
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Users.Commands.UpdateUser;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
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
        public async Task GivenValidUserRequest_WhenTheUserExistsAndUpdatesEmail_ReturnsUpdateUserViewModelResponseAndIssuesNewToken()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Email = "myUpdatedEmail@gmail.com",
                    Bio = "My updated bio",
                    Image = "Something super duper sexy"
                }
            };
            var originalUser = await UserManager.FindByEmailAsync(TestConstants.TestUserEmail);

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);
            var result = await command.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.User.Email.ShouldBe(updateUserCommand.User.Email);
            result.User.Bio.ShouldBe(updateUserCommand.User.Bio);
            result.User.Image.ShouldBe(updateUserCommand.User.Image);

            // Validate a new token was created
            result.User.Token.ShouldNotBe(new CurrentUserContextTest(UserManager).GetCurrentUserToken());
            result.User.Token.ShouldBe(new TokenServiceTest().CreateToken(originalUser));
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUserExistsAndUpdatesUsername_ReturnsUpdateUserViewModelResponseAndIssuesNewToken()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Username = "myUpdatedUsername",
                    Bio = "My updated bio",
                    Image = "Something super duper sexy"
                }
            };
            var originalUser = await UserManager.FindByEmailAsync(TestConstants.TestUserEmail);

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);
            var result = await command.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.User.Email.ShouldBe(TestConstants.TestUserEmail);
            result.User.Username.ShouldBe(updateUserCommand.User.Username);
            result.User.Bio.ShouldBe(updateUserCommand.User.Bio);
            result.User.Image.ShouldBe(updateUserCommand.User.Image);

            // Validate a new token was created
            result.User.Token.ShouldNotBe(new CurrentUserContextTest(UserManager).GetCurrentUserToken());
            result.User.Token.ShouldBe(new TokenServiceTest().CreateToken(originalUser));
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUserExistsAndDoesNotUpdateUsernameOrEmail_ReturnsUpdateUserViewModelResponseWithSameToken()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Bio = "My updated bio",
                    Image = "Something super duper sexy"
                }
            };
            var originalUser = await UserManager.FindByEmailAsync(TestConstants.TestUserEmail);

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);
            var result = await command.Handle(updateUserCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.User.ShouldNotBeNull();
            result.User.Email.ShouldBe(TestConstants.TestUserEmail);
            result.User.Username.ShouldBe(TestConstants.TestUserName);
            result.User.Bio.ShouldBe(updateUserCommand.User.Bio);
            result.User.Image.ShouldBe(updateUserCommand.User.Image);

            // Validate a new token was created
            result.User.Token.ShouldBe(new CurrentUserContextTest(UserManager).GetCurrentUserToken());
            result.User.Token.ShouldNotBe(new TokenServiceTest().CreateToken(originalUser));
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenTheUpdatedUsernameAlreadyExists_ThrowsConduitApiExceptionForBadRequest()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Username = "joey.mckenzie",
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);

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
                User = new UserUpdateDto
                {
                    Email = "joey.mckenzie@gmail.com",
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);

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
                User = new UserUpdateDto
                {
                    Email = string.Empty,
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);

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
                User = new UserUpdateDto
                {
                    Username = string.Empty,
                }
            };

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await command.Handle(updateUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidUserRequest_WhenRequestContainsANewPassword_ReturnsUserViewModelWithSuccessfulResponse()
        {
            // Arrange, grab a reference to the previous user
            var updateUserCommand = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Bio = "You feelin' lucky, punk?",
                    Password = "myNewPassword123"
                }
            };
            var originalUser = await UserManager.FindByEmailAsync(TestConstants.TestUserEmail);

            // Act
            var command = new UpdateUserCommandHandler(CurrentUserContext, Mapper, UserManager, Context, TokenService);
            var result = await command.Handle(updateUserCommand, CancellationToken.None);

            // Assert, validate the password hash has been updated
            result.ShouldNotBeNull();
            result.ShouldBeOfType<UserViewModel>();
            result.User.ShouldNotBeNull();
            result.User.Bio.ShouldBe(updateUserCommand.User.Bio);

            // Validate the updated user
            var updatedUser = await UserManager.FindByEmailAsync(TestConstants.TestUserEmail);
            updatedUser.Bio.ShouldBe(updateUserCommand.User.Bio);
            updatedUser.PasswordHash.ShouldNotBeNullOrWhiteSpace();
            var validatedPassword = await UserManager.CheckPasswordAsync(originalUser, updateUserCommand.User.Password);
            var invalidOldPassword = await UserManager.CheckPasswordAsync(originalUser, "#passwordTwo1!");
            validatedPassword.ShouldBe(true);
            invalidOldPassword.ShouldBe(false);
        }
    }
}