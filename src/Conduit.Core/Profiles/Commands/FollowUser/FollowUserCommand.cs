namespace Conduit.Core.Profiles.Commands.FollowUser
{
    using Domain.ViewModels;
    using MediatR;

    public class FollowUserCommand : IRequest<ProfileViewModel>
    {
        public FollowUserCommand(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}