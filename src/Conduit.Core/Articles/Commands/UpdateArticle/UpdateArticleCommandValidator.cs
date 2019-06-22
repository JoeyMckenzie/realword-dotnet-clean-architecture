namespace Conduit.Core.Articles.Commands.UpdateArticle
{
    using FluentValidation;

    public class UpdateArticleCommandValidator : AbstractValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(a => a.Article)
                .NotNull();
        }
    }
}