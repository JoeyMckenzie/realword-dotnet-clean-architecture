namespace Conduit.Core.Tests.Profiles
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Profiles.Commands.FollowUser;
    using Core.Profiles.Commands.UnfollowUser;
    using Domain.Dtos;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UnfollowUserCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheFollowerExists_ReturnsProfileViewModelAndRemovesFollowee()
        {
            // Arrange, verify the user is currently being followed by the requester
            var unfollowUserCommand = new UnfollowUserCommand("joey.mckenzie");
            var userFollowee = Context.Users.FirstOrDefault(u => u.UserName == "joey.mckenzie");
            var userFollower = Context.Users.FirstOrDefault(u => u.UserName == "test.user");
            userFollowee.ShouldNotBeNull();
            userFollowee.Followers.ShouldContain(u => u.UserFollower == userFollower);
            userFollower.ShouldNotBeNull();
            userFollower.Following.ShouldContain(u => u.UserFollowing == userFollowee);

            // Act
            var request = new UnfollowUserCommandHandler(CurrentUserContext, UserManager, Context, Mapper);
            var response = await request.Handle(unfollowUserCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ProfileViewModel>();
            response.Profile.ShouldNotBeNull();
            response.Profile.ShouldBeOfType<ProfileDto>();
            userFollowee.Followers.ShouldNotContain(u => u.UserFollower == userFollowee);
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
        public async Task GivenValidRequest_WhenTheFollowerTriesToUnfollowThemselves_ThrowsApiException()
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