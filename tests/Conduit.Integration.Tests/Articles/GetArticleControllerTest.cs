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

    public class GetArticleControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsArticleViewModel()
        {
            // Arrange/Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/how-to-train-your-dragon");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModel>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModel>();
            responseContent.Article.ShouldNotBeNull();
            responseContent.Article.ShouldBeOfType<ArticleDto>();
            responseContent.Article.Author.Username.ShouldBe("joey.mckenzie");
            responseContent.Article.TagList.ShouldNotBeEmpty();
            responseContent.Article.TagList.ShouldContain("dragons");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorViewModelWithNotFound()
        {
            // Arrange/Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/this-article-does-not-exist");
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