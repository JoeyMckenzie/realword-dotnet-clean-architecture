namespace Conduit.Integration.Tests.Profiles
{
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetProfilesCommandControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheUserExists_ReturnsProfileViewModelWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.GetAsync($"{ProfilesEndpoint}/joey.mckenzie");
            var responseContent = await ContentHelper.GetResponseContent<ProfileViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ProfileViewModel>();
            responseContent.Profile.ShouldNotBeNull();
            responseContent.Profile.ShouldBeOfType<ProfileDto>();
            responseContent.Profile.Username.ShouldBe("joey.mckenzie");
            responseContent.Profile.Following.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.GetAsync($"{ProfilesEndpoint}/thisUserDoesNotExist");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }
    }
}