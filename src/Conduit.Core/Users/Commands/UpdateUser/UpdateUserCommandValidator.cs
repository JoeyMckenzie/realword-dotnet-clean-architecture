namespace Conduit.Core.Users.Commands.UpdateUser
{
    using FluentValidation;

    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.User)
                .NotNull();

            RuleFor(u => u.User.Email)
                .EmailAddress();
        }
    }
}