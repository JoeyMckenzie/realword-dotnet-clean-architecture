namespace Conduit.Core.Articles.Commands.DeleteArticle
{
    using FluentValidation;

    public class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
    {
        public DeleteArticleCommandValidator()
        {
        }
    }
}