namespace Conduit.Core.Users.Commands.LoginUser
{
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
    using MediatR;

    public class LoginUserCommand : IRequest<UserViewModel>
    {
        public UserLoginDto User { get; set; }
    }
}