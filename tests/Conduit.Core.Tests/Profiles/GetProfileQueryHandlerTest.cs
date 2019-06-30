namespace Conduit.Core.Tests.Profiles
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Profiles.Queries.GetProfile;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetProfileQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheUserExists_ReturnsProfileViewModel()
        {
            // Arrange
            var getProfileRequest = new GetProfileQuery("joey.mckenzie");

            // Act
            var request = new GetProfileQueryQueryHandler(Mapper, Context, CurrentUserContext);
            var response = await request.Handle(getProfileRequest, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ProfileViewModel>();
            response.Profile.ShouldNotBeNull();
            response.Profile.ShouldBeOfType<ProfileDto>();
            response.Profile.Username.ShouldBe("joey.mckenzie");
            response.Profile.Following.ShouldBe(true);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserDoesNotExist_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            var getProfileRequest = new GetProfileQuery("aUserThatDoesNotExist");

            // Act
            var request = new GetProfileQueryQueryHandler(Mapper, Context, CurrentUserContext);

            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(getProfileRequest, CancellationToken.None);
            });
        }
    }
}