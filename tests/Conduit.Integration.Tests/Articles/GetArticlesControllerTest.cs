namespace Conduit.Integration.Tests.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetArticlesControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheRequestDoesNotContainAnyQueryParams_ReturnsArticlesViewModelWithAllArticles()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.Articles.ShouldNotBeEmpty();
            responseContent.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
            responseContent.Articles.ShouldContain(a => a.Author.Username == "test.user");
            responseContent.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.TagList.ShouldNotBeEmpty();
            responseContent.Articles.FirstOrDefault(a => a.Author.Username == "tes.user")?.TagList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestContainsQueryParams_ReturnsArticlesViewModelWithFilteredArticles()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}?tag=dragons&author=joey.mckenzie");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.Articles.ShouldNotBeEmpty();
            responseContent.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
            responseContent.Articles.ShouldNotContain(a => a.Author.Username == "test.user");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestContainsQueryParamsThatDoNotReturnResults_ReturnsArticlesViewModelWithNoArticles()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}?favorited=iDoNotExist");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.Articles.ShouldBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestContainsLimitParam_ReturnsArticlesViewModelWithLimitedResults()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}?limit=1");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.Articles.Count().ShouldBe(1);
            responseContent.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestContainsOffsetParam_ReturnsArticlesViewModelWithSkippedResults()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}?offset=1");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.Articles.Count().ShouldBe(1);
            responseContent.Articles.ShouldContain(a => a.Author.Username == "test.user");
        }
    }
}