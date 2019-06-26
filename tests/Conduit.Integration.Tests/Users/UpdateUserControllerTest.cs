namespace Conduit.Integration.Tests.Users
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Users.Commands.LoginUser;
    using Core.Users.Commands.UpdateUser;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UpdateUserControllerTest : ControllerBaseTestFixture
    {
        private const string UpdateUserEndpoint = "/api/user";

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUserIsFoundAndUpdatesEmail_ReturnsUserViewModelWithSuccessfulResponseAndNewToken()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Email = "my.new.email@gmail.com",
                    Password = "123myNewPassword!",
                }
            };
            var userLoginRequestWithNewEmailAndPassword = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = updateUserRequest.User.Email,
                    Password = updateUserRequest.User.Password
                }
            };
            await ContentHelper.GetRequestWithAuthorization(Client);
            var request = ContentHelper.GetRequestContent(updateUserRequest);
            var token = Client.DefaultRequestHeaders.Authorization.ToString().Split(" ")[1];

            // Act
            var response = await Client.PutAsync(UpdateUserEndpoint, request);
            var responseContent = await ContentHelper.GetResponseContent<UserViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<UserViewModel>();
            responseContent.User.ShouldNotBeNull();
            responseContent.User.Email.ShouldBe(updateUserRequest.User.Email);

            // Validate a new token was issued to the user
            responseContent.User.Token.ShouldNotBeNullOrWhiteSpace();
            responseContent.User.Token.ShouldNotBe(token);

            // Validate the user can login with new password
            var loginRequest = ContentHelper.GetRequestContent(userLoginRequestWithNewEmailAndPassword);
            var loginResponse = await Client.PostAsync("/api/users/login", loginRequest);
            var loginResponseContent = await ContentHelper.GetResponseContent<UserViewModel>(loginResponse);
            loginResponse.EnsureSuccessStatusCode();
            loginResponseContent.User.ShouldNotBeNull();
            loginResponseContent.User.Email.ShouldBe(updateUserRequest.User.Email);
            loginResponseContent.User.Username.ShouldBe("joey.mckenzie");
        }

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUserIsFoundAndUpdatesUsername_ReturnsUserViewModelWithSuccessfulResponseAndNewToken()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Username = "my.new.username",
                    Password = "123myNewPassword!",
                }
            };
            var userLoginRequestWithNewEmailAndPassword = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = IntegrationTestConstants.PrimaryUser.User.Email,
                    Password = updateUserRequest.User.Password
                }
            };
            await ContentHelper.GetRequestWithAuthorization(Client);
            var request = ContentHelper.GetRequestContent(updateUserRequest);
            var token = Client.DefaultRequestHeaders.Authorization.ToString().Split(" ")[1];

            // Act
            var response = await Client.PutAsync(UpdateUserEndpoint, request);
            var responseContent = await ContentHelper.GetResponseContent<UserViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<UserViewModel>();
            responseContent.User.ShouldNotBeNull();
            responseContent.User.Email.ShouldBe(IntegrationTestConstants.PrimaryUser.User.Email);
            responseContent.User.Username.ShouldBe(updateUserRequest.User.Username);

            // Validate a new token was issued to the user
            responseContent.User.Token.ShouldNotBeNullOrWhiteSpace();
            responseContent.User.Token.ShouldNotBe(token);

            // Validate the user can login with new password
            var loginRequest = ContentHelper.GetRequestContent(userLoginRequestWithNewEmailAndPassword);
            var loginResponse = await Client.PostAsync("/api/users/login", loginRequest);
            var loginResponseContent = await ContentHelper.GetResponseContent<UserViewModel>(loginResponse);
            loginResponse.EnsureSuccessStatusCode();
            loginResponseContent.User.ShouldNotBeNull();
            loginResponseContent.User.Email.ShouldBe(IntegrationTestConstants.PrimaryUser.User.Email);
            loginResponseContent.User.Username.ShouldBe(updateUserRequest.User.Username);
        }

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUserIsFoundAndDoesNotUpdateEmailOrUsername_ReturnsUserViewModelWithSuccessfulResponse()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Bio = "This is my new bio!",
                    Password = "123myNewPassword!",
                }
            };
            var userLoginRequestWithNewEmailAndPassword = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = IntegrationTestConstants.PrimaryUser.User.Email,
                    Password = updateUserRequest.User.Password
                }
            };
            await ContentHelper.GetRequestWithAuthorization(Client);
            var request = ContentHelper.GetRequestContent(updateUserRequest);
            var token = Client.DefaultRequestHeaders.Authorization.ToString().Split(" ")[1];

            // Act
            var response = await Client.PutAsync(UpdateUserEndpoint, request);
            var responseContent = await ContentHelper.GetResponseContent<UserViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<UserViewModel>();
            responseContent.User.ShouldNotBeNull();
            responseContent.User.Bio.ShouldBe(updateUserRequest.User.Bio);
            responseContent.User.Email.ShouldBe(IntegrationTestConstants.PrimaryUser.User.Email);
            responseContent.User.Username.ShouldBe("joey.mckenzie");

            // Validate a the same token was returned on the response
            responseContent.User.Token.ShouldNotBeNullOrWhiteSpace();
            responseContent.User.Token.ShouldBe(token);

            // Validate the user can login with new password
            var loginRequest = ContentHelper.GetRequestContent(userLoginRequestWithNewEmailAndPassword);
            var loginResponse = await Client.PostAsync("/api/users/login", loginRequest);
            var loginResponseContent = await ContentHelper.GetResponseContent<UserViewModel>(loginResponse);
            loginResponse.EnsureSuccessStatusCode();
            loginResponseContent.User.ShouldNotBeNull();
            loginResponseContent.User.Bio.ShouldBe(updateUserRequest.User.Bio);
            loginResponseContent.User.Email.ShouldBe(IntegrationTestConstants.PrimaryUser.User.Email);
            loginResponseContent.User.Username.ShouldBe("joey.mckenzie");
        }

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUpdatedUsernameIsInUse_ReturnsErrorViewModelWithBadRequest()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Email = "my.new.email@gmail.com",
                    Password = "123myNewPassword!",
                    Username = "test.user"
                }
            };
            await ContentHelper.GetRequestWithAuthorization(Client);
            var request = ContentHelper.GetRequestContent(updateUserRequest);

            // Act
            var response = await Client.PutAsync(UpdateUserEndpoint, request);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenValidUserUpdateRequest_WhenTheUpdatedEmailIsInUse_ReturnsErrorViewModelWithBadRequest()
        {
            // Arrange
            var updateUserRequest = new UpdateUserCommand
            {
                User = new UserUpdateDto
                {
                    Email = "test.user@gmail.com",
                    Password = "123myNewPassword!",
                    Username = "my.new.username"
                }
            };
            await ContentHelper.GetRequestWithAuthorization(Client);
            var request = ContentHelper.GetRequestContent(updateUserRequest);

            // Act
            var response = await Client.PutAsync(UpdateUserEndpoint, request);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}