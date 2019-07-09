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

    public class FavoriteArticleControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/favorite", null);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.ShouldBeOfType<ArticleDto>();
            responseContent.Article.Favorited.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheUserHasAlreadyFavoritedTheArticle_ReturnsArticleViewModelWithSuccessfulResponse()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            await Client.PostAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/favorite", null);
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-train-your-dragon/favorite", null);
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.ShouldBeOfType<ArticleDto>();
            responseContent.Article.Favorited.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.PostAsync($"{ArticlesEndpoint}/how-to-not-train-your-dragon/favorite", null);
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