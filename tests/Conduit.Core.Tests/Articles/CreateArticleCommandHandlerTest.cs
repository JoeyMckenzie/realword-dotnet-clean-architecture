namespace Conduit.Core.Tests.Articles
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.CreateArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
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
                Article = new CreateArticleDto
                {
                    Title = "Why C# is the Best Language",
                    Description = "It really is!",
                    Body = "I love .NET Core",
                    TagList = new[] { "dotnet", "c#" }
                }
            };

            // Act
            var request = new CreateArticleCommandHandler(CurrentUserContext, Context, _logger, Mapper, MachineDateTime);
            var result = await request.Handle(createArticleCommand, CancellationToken.None);

            // Assert the response model
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ArticleViewModel>();
            result.Article.ShouldNotBeNull();
            result.Article.Slug.ShouldBe("why-c-is-the-best-language");
            result.Article.FavoritesCount.ShouldBe(0);
            result.Article.TagList.ShouldContain("dotnet");
            result.Article.TagList.ShouldContain("c#");
            result.Article.Author.Username.ShouldBe(TestConstants.TestUserName);

            // Assert the article within the DB context
            var newArticle = Context.Articles.FirstOrDefault(a => string.Equals(a.Slug, result.Article.Slug, StringComparison.OrdinalIgnoreCase));
            newArticle.ShouldNotBeNull();
            newArticle.Slug.ShouldBe("why-c-is-the-best-language");
            newArticle.FavoritesCount.ShouldBe(0);
            newArticle.Author.Email.ShouldBe(TestConstants.TestUserEmail);
        }

        [Fact]
        public async Task GivenValidArticleRequest_WhenTheRequestDoesNotContainTagList_ReturnsSuccessfulResponseWithViewModel()
        {
            // Arrange
            var createArticleCommand = new CreateArticleCommand
            {
                Article = new CreateArticleDto
                {
                    Title = "Why C# is the Best Language",
                    Description = "It really is!",
                    Body = "I love .NET Core",
                }
            };

            // Act
            var request = new CreateArticleCommandHandler(CurrentUserContext, Context, _logger, Mapper, new DateTimeTest());
            var result = await request.Handle(createArticleCommand, CancellationToken.None);

            // Assert the response model
            result.ShouldNotBeNull();
            result.ShouldBeOfType<ArticleViewModel>();
            result.Article.ShouldNotBeNull();
            result.Article.Slug.ShouldBe("why-c-is-the-best-language");
            result.Article.FavoritesCount.ShouldBe(0);
            result.Article.TagList.ShouldBeEmpty();
            result.Article.Author.Username.ShouldBe(TestConstants.TestUserName);

            // Assert the article within the DB context
            var newArticle = Context.Articles.FirstOrDefault(a => string.Equals(a.Slug, result.Article.Slug, StringComparison.OrdinalIgnoreCase));
            newArticle.ShouldNotBeNull();
            newArticle.Slug.ShouldBe("why-c-is-the-best-language");
            newArticle.FavoritesCount.ShouldBe(0);
            newArticle.Author.Email.ShouldBe(TestConstants.TestUserEmail);
            Context.ArticleTags.SingleOrDefault(at => at.ArticleId == newArticle.Id)?.ShouldBeNull();
        }
    }
}