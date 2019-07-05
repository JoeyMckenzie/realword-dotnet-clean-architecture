namespace Conduit.Integration.Tests.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetArticleCommentsControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsCommentViewModelListWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments");
            var responseContent = await ContentHelper.GetResponseContent<CommentViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<CommentViewModelList>();
            responseContent.Comments.ShouldNotBeNull();
            responseContent.Comments.ShouldBeOfType<List<CommentDto>>();
            responseContent.Comments.Count().ShouldBe(2);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExistsWithNoComments_ReturnsEmptyCommentViewModelListWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/why-beer-is-gods-gift-to-the-world/comments");
            var responseContent = await ContentHelper.GetResponseContent<CommentViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<CommentViewModelList>();
            responseContent.Comments.ShouldNotBeNull();
            responseContent.Comments.ShouldBeOfType<List<CommentDto>>();
            responseContent.Comments.ShouldBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/this-is-a-bad-article/comments");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}