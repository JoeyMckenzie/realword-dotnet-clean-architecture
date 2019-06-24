namespace Conduit.Integration.Tests.Users
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Api;
    using Core.Users.Commands.CreateUser;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class RegisterUsersControllerTest : ControllerBaseTestFixture
    {
        private const string RegisterEndpoint = "/api/users";

        [Fact]
        public async Task GivenAUserCreateRequest_WhenThePayloadIsValid_ReturnsUserViewModelWithSuccessfulResponse()
        {
            // Arrange
            var createUserRequest = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Email = "keanu.reeves@gmail.com",
                    Username = "JohnWick",
                    Password = "#johnWick123!"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(createUserRequest);

            // Act
            var response = await Client.PostAsync(RegisterEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<UserViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<UserViewModel>();
            responseContent.User.Email.ShouldBe(createUserRequest.User.Email);
            responseContent.User.Username.ShouldBe(createUserRequest.User.Username);
            responseContent.User.Token.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenAUserCreateRequest_WhenTheUserAlreadyExists_ReturnsErrorResponse()
        {
            // Arrange
            var createUserRequest = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Email = "joey.mckenzie@gmail.com",
                    Username = "joey.mckenzie",
                    Password = "#johnWick123!"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(createUserRequest);

            // Act
            var response = await Client.PostAsync(RegisterEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.ShouldNotBeNull();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenAUserCreateRequest_WhenRequestIsMissingProperties_ReturnsErrorResponseForValidation()
        {
            // Arrange
            var createUserRequest = new CreateUserCommand
            {
                User = new UserRegistrationDto
                {
                    Username = "JohnWick",
                    Password = "#johnWick123!"
                }
            };
            var requestContent = ContentHelper.GetRequestContent(createUserRequest);

            // Act
            var response = await Client.PostAsync(RegisterEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.ShouldNotBeNull();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}