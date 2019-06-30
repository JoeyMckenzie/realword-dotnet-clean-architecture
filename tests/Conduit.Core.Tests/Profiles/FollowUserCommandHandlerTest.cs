namespace Conduit.Core.Tests.Profiles
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Profiles.Commands.FollowUser;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class FollowUserCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheFollowerExists_ReturnsProfileViewModelAndAddsFollowee()
        {
            // Arrange, verify the user is not currently being followed by the requester
            var followUserCommand = new FollowUserCommand("test.user2");
            var user2 = Context.Users.FirstOrDefault(u => u.UserName == "test.user2");
            user2.ShouldNotBeNull();
            user2.Followers.ShouldBeEmpty();

            // Act
            var request = new FollowUserCommandHandler(CurrentUserContext, Context, Mapper, UserManager, new DateTimeTest());
            var response = await request.Handle(followUserCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ProfileViewModel>();
            response.Profile.ShouldNotBeNull();
            response.Profile.ShouldBeOfType<ProfileDto>();
            user2.Followers.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheFollowerDoesNotExist_ThrowsApiException()
        {
            // Arrange, verify the user is not currently being followed by the requester
            var followUserCommand = new FollowUserCommand("this.user.does.not.exist");

            // Act
            var request = new FollowUserCommandHandler(CurrentUserContext, Context, Mapper, UserManager, new DateTimeTest());

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(followUserCommand, CancellationToken.None);
            });
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheFollowerTriesToFollowThemselves_ThrowsApiException()
        {
            // Arrange, verify the user is not currently being followed by the requester
            var followUserCommand = new FollowUserCommand(TestConstants.TestUserName);

            // Act
            var request = new FollowUserCommandHandler(CurrentUserContext, Context, Mapper, UserManager, new DateTimeTest());

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(followUserCommand, CancellationToken.None);
            });
        }
    }
}