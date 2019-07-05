namespace Conduit.Core.Tests.Articles
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Queries.GetCommentsFromArticle;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class ArticleCommentsQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsCommentViewModelListWithComments()
        {
            // Arrange
            var articleCommentsQuery = new ArticleCommentsQuery("how-to-train-your-dragon");

            // Act
            var handler = new ArticleCommentsQueryHandler(Context, Mapper);
            var response = await handler.Handle(articleCommentsQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<CommentViewModelList>();
            response.Comments.Count().ShouldBe(2);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ThrowsApiExceptionWithNotFound()
        {
            // Arrange
            var articleCommentsQuery = new ArticleCommentsQuery("how-to-not-train-your-dragon");

            // Act
            var handler = new ArticleCommentsQueryHandler(Context, Mapper);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(articleCommentsQuery, CancellationToken.None);
            });

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ConduitApiException>();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotContainAnyComments_ReturnsEmptyCommentViewModelList()
        {
            // Arrange
            var articleCommentsQuery = new ArticleCommentsQuery("why-beer-is-gods-gift-to-the-world");

            // Act
            var handler = new ArticleCommentsQueryHandler(Context, Mapper);
            var response = await handler.Handle(articleCommentsQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<CommentViewModelList>();
            response.Comments.ShouldBeEmpty();
        }
    }
}