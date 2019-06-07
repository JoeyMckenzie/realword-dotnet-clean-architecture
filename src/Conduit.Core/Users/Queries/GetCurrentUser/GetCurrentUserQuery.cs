namespace Conduit.Core.Users.Queries.GetCurrentUser
{
    using Domain.ViewModels;
    using MediatR;

    public class GetCurrentUserQuery : IRequest<UserViewModel>
    {
    }
}