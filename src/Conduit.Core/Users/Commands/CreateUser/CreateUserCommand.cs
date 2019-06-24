namespace Conduit.Core.Users.Commands.CreateUser
{
    using Domain.Dtos;
    using Domain.Dtos.Users;
    using Domain.ViewModels;
    using MediatR;

    public class CreateUserCommand : IRequest<UserViewModel>
    {
        public UserRegistrationDto User { get; set; }
    }
}