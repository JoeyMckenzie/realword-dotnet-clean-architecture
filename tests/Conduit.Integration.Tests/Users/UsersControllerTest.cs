namespace Conduit.Integration.Tests.Users
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Api;
    using Core.Users.Commands.CreateUser;
    using Core.Users.Commands.LoginUser;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UsersControllerTest : IClassFixture<ConduitWebApplicationFactory<Startup>>
    {
        private const string UsersEndpoint = "/api/users";
        private readonly HttpClient _httpClient;

        public UsersControllerTest(ConduitWebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();
        }

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
            var response = await _httpClient.PostAsync(UsersEndpoint, requestContent);
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
            var response = await _httpClient.PostAsync(UsersEndpoint, requestContent);
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
            var response = await _httpClient.PostAsync(UsersEndpoint, requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.ShouldNotBeNull();
            responseContent.Errors.ShouldNotBeNull();
        }

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
            var response = await _httpClient.PostAsync($"{UsersEndpoint}/login", requestContent);
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
            var response = await _httpClient.PostAsync($"{UsersEndpoint}/login", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
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
            var response = await _httpClient.PostAsync($"{UsersEndpoint}/login", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}