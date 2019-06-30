namespace Conduit.Core.Profiles.Commands.FollowUser
{
    using FluentValidation;

    public class FollowUserCommandValidator : AbstractValidator<FollowUserCommand>
    {
        public FollowUserCommandValidator()
        {
            RuleFor(c => c.Username)
                .NotEmpty();
        }
    }
}