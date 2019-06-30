namespace Conduit.Core.Profiles.Commands.UnfollowUser
{
    using FluentValidation;

    public class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        public UnfollowUserCommandValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty();
        }
    }
}