namespace Conduit.Core.Articles.Commands.AddComment
{
    using FluentValidation;

    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(c => c.Comment)
                .NotNull();

            RuleFor(c => c.Comment.Body)
                .NotNull();

            RuleFor(c => c.Slug)
                .NotEmpty()
                .NotNull();
        }
    }
}