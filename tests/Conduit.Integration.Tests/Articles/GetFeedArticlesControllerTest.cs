namespace Conduit.Integration.Tests.Articles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetFeedArticlesControllerTest : ControllerBaseTestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenUserIsFollowingOtherUsers_ReturnsListOfFeedArticles()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/feed");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldNotBeEmpty();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
            responseContent.ArticlesCount.ShouldBe(1);
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsQueryParams_ReturnsLimitedListOfFeedArticles()
        {
            // Arrange
            await ContentHelper.GetRequestWithAuthorization(Client, IntegrationTestConstants.SecondaryUser);

            // Act
            var response = await Client.GetAsync($"{ArticlesEndpoint}/feed?offset=1");
            var responseContent = await ContentHelper.GetResponseContent<ArticleViewModelList>(response);

            // Assert
            response.EnsureSuccessStatusCode();
            responseContent.ShouldNotBeNull();
            responseContent.ShouldBeOfType<ArticleViewModelList>();
            responseContent.Articles.ShouldNotBeNull();
            responseContent.Articles.ShouldBeEmpty();
            responseContent.Articles.ShouldBeOfType<List<ArticleDto>>();
        }
    }
}