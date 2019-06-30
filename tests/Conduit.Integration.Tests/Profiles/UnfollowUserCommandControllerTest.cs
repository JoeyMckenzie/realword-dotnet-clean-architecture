namespace Conduit.Integration.Tests.Profiles
{
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UnfollowUserCommandControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheUserExists_ReturnsProfileViewModelAndFollowingStatusOfFalse()
        {
            // Arrange
            var userUnfollowEndpoint = $"{ProfilesEndpoint}/joey.mckenzie/follow";
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync(userUnfollowEndpoint);
            var responseContent = await ContentHelper.GetResponseContent<ProfileViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ProfileViewModel>();
            responseContent.Profile.ShouldNotBeNull();
            responseContent.Profile.ShouldBeOfType<ProfileDto>();
            responseContent.Profile.Following.ShouldBeFalse();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var userUnfollowEndpoint = $"{ProfilesEndpoint}/joey.mckenzie.is.a.clone/follow";
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync(userUnfollowEndpoint);
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
            var userUnfollowEndpoint = $"{ProfilesEndpoint}/test.user/follow";
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync(userUnfollowEndpoint);
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
            var userUnfollowEndpoint = $"{ProfilesEndpoint}/ /follow";
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync(userUnfollowEndpoint);
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