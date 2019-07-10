namespace Conduit.Core.Articles.Commands.UnfavoriteArticle
{
    using FluentValidation;

    public class UnfavoriteArticleCommandValidator : AbstractValidator<UnfavoriteArticleCommand>
    {
        public UnfavoriteArticleCommandValidator()
        {
            RuleFor(f => f.Slug)
                .NotNull()
                .NotEmpty();
        }
    }
}