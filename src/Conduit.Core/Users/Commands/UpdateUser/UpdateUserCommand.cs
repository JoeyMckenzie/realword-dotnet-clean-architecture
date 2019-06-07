namespace Conduit.Core.Users.Commands.UpdateUser
{
    using Domain.ViewModels;
    using MediatR;

    public class UpdateUserCommand : IRequest<UserViewModel>
    {
    }
}