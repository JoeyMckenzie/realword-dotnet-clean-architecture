namespace Conduit.Core.Tests.Articles
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.DeleteArticle;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shared.Extensions;
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

            // Assert
        }
    }
}