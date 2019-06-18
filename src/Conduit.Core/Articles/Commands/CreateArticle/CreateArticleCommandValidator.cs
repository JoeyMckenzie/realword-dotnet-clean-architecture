namespace Conduit.Core.Articles.Commands.CreateArticle
{
    using FluentValidation;

    public class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
    {
        public CreateArticleCommandValidator()
        {
            RuleFor(a => a.Article)
                .NotNull();

            RuleFor(a => a.Article.Title)
                .NotEmpty();

            RuleFor(a => a.Article.Description)
                .NotEmpty();

            RuleFor(a => a.Article.Body)
                .NotEmpty();
        }
    }
}