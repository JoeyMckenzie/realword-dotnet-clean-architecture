namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Articles.Commands.UpdateArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UpdateArticlesControllerTest : ControllerBaseTestFixture
    {
        private const string UpdateEndpoint = "/api/articles";

        [Fact]
        public async Task GivenValidUpdateRequest_WhenTheArticleExistsAndIsOwnedByTheUser_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "how-to-train-your-dragon",
                Article = new UpdateArticleDto
                {
                    Body = "This is the new body!",
                    Title = "My new title!"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(updateArticleCommand, Client);

            // Act
            var response = await Client.PutAsync($"{UpdateEndpoint}/{updateArticleCommand.Slug}", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.Body.ShouldBe(updateArticleCommand.Article.Body);
            responseContent.Article.Title.ShouldBe(updateArticleCommand.Article.Title);
            responseContent.Article.Slug.ShouldBe("my-new-title");
        }

        [Fact]
        public async Task GivenValidUpdateRequest_WhenTheArticleExistsAndIsNotOwnedByTheUser_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "how-to-train-your-dragon",
                Article = new UpdateArticleDto
                {
                    Body = "This is the new body!",
                    Title = "My new title!"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(updateArticleCommand, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PutAsync($"{UpdateEndpoint}/{updateArticleCommand.Slug}", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenValidUpdateRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "this-is-not-an-article",
                Article = new UpdateArticleDto
                {
                    Body = "This is the new body!",
                    Title = "My new title!"
                }
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(updateArticleCommand, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PutAsync($"{UpdateEndpoint}/{updateArticleCommand.Slug}", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenValidUpdateRequest_WhenTheArticleDtoIsInvalid_ReturnsErrorViewModelWithUnsupportedMediaType()
        {
            // Arrange
            var updateArticleCommand = new UpdateArticleCommand
            {
                Slug = "this-is-not-an-article"
            };
            var requestContent = await ContentHelper.GetRequestContentWithAuthorization(updateArticleCommand, Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PutAsync($"{UpdateEndpoint}/{updateArticleCommand.Slug}", requestContent);
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.UnsupportedMediaType);
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}