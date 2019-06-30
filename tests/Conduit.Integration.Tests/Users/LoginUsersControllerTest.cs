namespace Conduit.Integration.Tests.Users
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class LoginUsersControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenAUserLoginRequest_WhenUserExistsAndPasswordIsValid_ReturnsUserViewModelAndSuccessfulResponse()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "joey.mckenzie@gmail.com",
                    Password = "#password1!"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(userLoginCommand);

            // Act
            var response = await Client.PostAsync(LoginEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<UserViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<UserViewModel>();
            responseContent.User.Email.ShouldBe(userLoginCommand.User.Email);
            responseContent.User.Token.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenAUserLoginRequest_WhenUserExistsAndPasswordIsInvalid_ReturnsErrorViewModelWithBadRequest()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "joey.mckenzie@gmail.com",
                    Password = "wrongPasswordDummy"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(userLoginCommand);

            // Act
            var response = await Client.PostAsync(LoginEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenAUserLoginRequest_WhenUserDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "keanu.reeves@gmail.com",
                    Password = "billAndTedsExcellentAdventure!"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(userLoginCommand);

            // Act
            var response = await Client.PostAsync(LoginEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenAUserLoginRequest_WhenRequestIsInvalid_ReturnsErrorViewModelWithUnsupportedMediaType()
        {
            // Arrange
            var userLoginCommand = new LoginUserCommand
            {
                User = new UserLoginDto
                {
                    Email = "joey.mckenzie@gmail.com",
                }
            };
            var requestContent = ContentHelper.GetRequestContent(userLoginCommand);

            // Act
            var response = await Client.PostAsync(LoginEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}