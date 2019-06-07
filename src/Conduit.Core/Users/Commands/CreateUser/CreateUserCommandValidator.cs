namespace Conduit.Core.Users.Commands.CreateUser
{
    using FluentValidation;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.User)
                .NotNull();

            RuleFor(u => u.User.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(u => u.User.Username)
                .NotEmpty();

            RuleFor(u => u.User.Password)
                .NotEmpty();
        }
    }
}