namespace Conduit.Core.Tests.Articles
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.CreateArticle;
    using Domain.Dtos;
    using Infrastructure;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Shouldly;
    using Xunit;

    public class CreateArticleCommandHandlerTest : TestFixture
    {
        private readonly ILogger<CreateArticleCommandHandler> _logger;

        public CreateArticleCommandHandlerTest()
        {
            _logger = NullLogger<CreateArticleCommandHandler>.Instance;
        }

        [Fact]
        public async Task GivenValidArticleRequest_WhenTheArticleHasNotPreviousBeenCreated_ReturnsSuccessfulResponseWithViewModel()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new ArticleDto
                {
                    Title = "How to train your dragon",
                    Description = "Ever wonder how?",
                    Body = "You have to believe",
                    TagList = new[] { "reactjs", "angularjs", "dragons" }
                }
            };

            // Act
            var request = new CreateArticleCommandHandler(CurrentUserContext, Context, _logger, Mapper, new DateTimeTest());
            var result = await request.Handle(createArticleCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
        }
    }
}