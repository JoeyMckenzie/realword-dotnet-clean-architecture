namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Core.Articles.Commands.DeleteArticle;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class DeleteArticlesControllerTest : ControllerBaseTestFixture
    {
        private const string ArticlesEndpoint = "/api/articles";

        [Fact]
        public async Task GivenValidDeleteArticle_WhenTheArticleExistsAndIsOwnedByTheUser_ReturnsSuccessfulResponse()
        {
            // Arrange
            var deleteArticleCommand = new DeleteArticleCommand("how-to-train-your-dragon");
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/{deleteArticleCommand.Slug}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenValidDeleteArticle_WhenTheArticleExistsAndIsNotOwnedByTheUser_ReturnsErrorViewModelForUnauthorized()
        {
            // Arrange
            var deleteArticleCommand = new DeleteArticleCommand("how-to-train-your-dragon");
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/{deleteArticleCommand.Slug}");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}