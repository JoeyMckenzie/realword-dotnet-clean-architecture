namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Articles.Commands.AddComment;
    using Domain.Dtos;
    using Domain.Dtos.Comments;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class AddCommentControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsCommentViewModel()
        {
            // Arrange
            var comment = new AddCommentCommand
            {
                Comment = new AddCommentDto
                {
                    Body = "You stink!"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(comment, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<CommentViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<CommentViewModel>();
            responseContent.Comment.ShouldNotBeNull();
            responseContent.Comment.ShouldBeOfType<CommentDto>();
            responseContent.Comment.Body.ShouldNotBeEmpty(comment.Comment.Body);
            responseContent.Comment.Author.Username.ShouldBe("test.user");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var comment = new AddCommentCommand
            {
                Comment = new AddCommentDto
                {
                    Body = "You stink!"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(comment, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-not-train-your-dragon/comments", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }

        [Fact]
        public async Task GivenInvalidRequest_WhenTheArticleDoesNotContainABody_ReturnsErrorViewModelWithUnsupportedMediaType()
        {
            // Arrange
            var comment = new AddCommentCommand
            {
                Comment = new AddCommentDto()
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(comment, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }
    }
}