namespace Conduit.Integration.Tests.Articles
{
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Dtos;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UnfavoriteArticleControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExistsAndUserHasFavorited_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/favorite");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.ShouldBeOfType<ArticleDto>();
            responseContent.Article.Favorited.ShouldBeFalse();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExistsAndUserHasNotFavorited_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/why-beer-is-gods-gift-to-the-world/favorite");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.ShouldBeOfType<ArticleDto>();
            responseContent.Article.Favorited.ShouldBeFalse();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.DeleteAsync($"{ArticlesEndpoint}/this-doesnt-exist/favorite");
            var responseContent = await ContentHelper.GetResponseContent<ErrorViewModel>(response);

            // Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ErrorViewModel>();
            responseContent.Errors.ShouldNotBeNull();
            responseContent.Errors.ShouldBeOfType<ErrorDto>();
        }
    }
}