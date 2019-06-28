namespace Conduit.Core.Profiles.Queries.GetProfile
{
    using Domain.ViewModels;
    using MediatR;

    public class GetProfileQuery : IRequest<ProfileViewModel>
    {
        public GetProfileQuery(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}