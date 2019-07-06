namespace Conduit.Core.Articles.Commands.DeleteComment
{
    using FluentValidation;

    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(c => c.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.Slug)
                .NotNull()
                .NotEmpty();
        }
    }
}