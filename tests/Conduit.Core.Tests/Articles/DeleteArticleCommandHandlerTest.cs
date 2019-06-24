namespace Conduit.Core.Tests.Articles
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.DeleteArticle;
    using Exceptions;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shared.Extensions;
    using Shouldly;
    using Xunit;

    public class DeleteArticleCommandHandlerTest : TestFixture
    {
        private readonly ILogger<DeleteArticleCommandHandler> _logger;

        public DeleteArticleCommandHandlerTest()
        {
            _logger = NullLogger<DeleteArticleCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenTheRequestIsValid_WhenTheUserIsTheAuthorOfArticle_DeletesArticleFromDbContext()
        {
            // Arrange
            var deleteArticleCommand = new DeleteArticleCommand("Why Beer is God's Gift to the World".ToSlug());

            // Act
            var request = new DeleteArticleCommandHandler(_logger, Context, CurrentUserContext);
            var result = await request.Handle(deleteArticleCommand, CancellationToken.None);

            // Assert, verify removal from the database
            result.ShouldNotBeNull();
            Context.Articles.FirstOrDefault(a => a.Slug == "Why Beer is God's Gift to the World".ToSlug())?.ShouldBeNull();
        }

        [Fact]
        public async Task GivenTheRequestIsValid_WhenTheUserDoesNotOwnTheArticle_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            var deleteArticleCommand = new DeleteArticleCommand("How to train your dragon".ToSlug());

            // Act
            var request = new DeleteArticleCommandHandler(_logger, Context, CurrentUserContext);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(deleteArticleCommand, CancellationToken.None);
            });
            Context.Articles.FirstOrDefault(a => a.Slug == "How to train your dragon".ToSlug()).ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenTheRequestIsValid_WhenTheArticleDoesNotExist_ThrowsApiExceptionForNotFound()
        {
            // Arrange
            var deleteArticleCommand = new DeleteArticleCommand("How to train your dragon".ToSlug());

            // Act
            var request = new DeleteArticleCommandHandler(_logger, Context, CurrentUserContext);

            // Assert
            await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await request.Handle(deleteArticleCommand, CancellationToken.None);
            });
            Context.Articles.FirstOrDefault(a => a.Slug == "How to train your dragon".ToSlug()).ShouldNotBeNull();
        }
    }
}