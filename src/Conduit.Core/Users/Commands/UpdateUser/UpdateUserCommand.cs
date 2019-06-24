namespace Conduit.Core.Users.Commands.UpdateUser
{
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
    using MediatR;

    public class UpdateUserCommand : IRequest<UserViewModel>
    {
        public UserDto User { get; set; }
    }
}