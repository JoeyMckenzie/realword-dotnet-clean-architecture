namespace Conduit.Core.Articles.Commands.FavoriteArticle
{
    using FluentValidation;

    public class FavoriteArticleCommandValidator : AbstractValidator<FavoriteArticleCommand>
    {
        public FavoriteArticleCommandValidator()
        {
            RuleFor(f => f.Slug)
                .NotNull()
                .NotEmpty();
        }
    }
}