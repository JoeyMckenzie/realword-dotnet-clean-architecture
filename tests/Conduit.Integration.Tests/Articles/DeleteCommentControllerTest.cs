namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class DeleteCommentControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheCommentExistsAndIsOwnedByTheRequester_ReturnsOKResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments/1");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheCommentDoesNotExistAndIsOwnedByTheRequester_ReturnsErrorViewModelForNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments/11");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheCommentExistsAndIsNotOwnedByTheRequester_ReturnsErrorViewModelForForbidden()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/comments/2");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelForNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/how-to-not-train-your-dragon/comments/1");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
        }
    }
}