namespace Conduit.Core.Users.Commands.LoginUser
{
    using FluentValidation;

    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.User)
                .NotNull();

            RuleFor(u => u.User.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.User.Password)
                .NotEmpty();
        }
    }
}