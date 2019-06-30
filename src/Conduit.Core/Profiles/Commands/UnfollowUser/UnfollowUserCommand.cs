namespace Conduit.Core.Profiles.Commands.UnfollowUser
{
    using Domain.ViewModels;
    using MediatR;

    public class UnfollowUserCommand : IRequest<ProfileViewModel>
    {
        public UnfollowUserCommand(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}