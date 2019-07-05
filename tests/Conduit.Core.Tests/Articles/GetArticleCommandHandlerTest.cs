namespace Conduit.Core.Tests.Articles
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Queries.GetArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetArticleCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsArticleViewModel()
        {
            // Arrange
            var getArticleQuery = new GetArticleQuery("how-to-train-your-dragon");

            // Act
            var handler = new GetArticleQueryHandler(Context, Mapper);
            var response = await handler.Handle(getArticleQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModel>();
            response.Article.ShouldNotBeNull();
            response.Article.ShouldBeOfType<ArticleDto>();
            response.Article.Author.Username.ShouldBe("joey.mckenzie");
            response.Article.TagList.ShouldNotBeEmpty();
            response.Article.TagList.ShouldContain("dragons");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ThrowsApiExpection()
        {
            // Arrange
            var getArticleQuery = new GetArticleQuery("an-article-does-not-exist");

            // Act
            var handler = new GetArticleQueryHandler(Context, Mapper);
            var exception = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(getArticleQuery, CancellationToken.None);
            });

            // Assert
            exception.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}