namespace Conduit.Integration.Tests.Profiles
{
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class FollowUserCommandControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheUserExists_ReturnsProfileViewModelAndFollowingStatusOfTrue()
        {
            // Arrange
            var userFollowEndpoint = $"{ProfilesEndpoint}/test.user/follow";
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.PostAsync(userFollowEndpoint, null);
            var responseContent = await ContentHelper.GetResponseContent<ProfileViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ProfileViewModel>();
            responseContent.Profile.ShouldNotBeNull();
            responseContent.Profile.ShouldBeOfType<ProfileDto>();
            responseContent.Profile.Following.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var userFollowEndpoint = $"{ProfilesEndpoint}/test.user.not.exists/follow";
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.PostAsync(userFollowEndpoint, null);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserTriesToFollowThemselves_ReturnsErrorViewModelWithBadRequest()
        {
            // Arrange
            var userFollowEndpoint = $"{ProfilesEndpoint}/joey.mckenzie/follow";
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.PostAsync(userFollowEndpoint, null);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }

        [Fact]
        public async Task GivenInvalidRequest_WhenTheUserDoesNotExistInTheRequestPath_ReturnsErrorViewModelWithUnsupportedMedia()
        {
            // Arrange
            var userFollowEndpoint = $"{ProfilesEndpoint}/ /follow";
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.PostAsync(userFollowEndpoint, null);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }
    }
}