namespace Conduit.Core.Tests.Articles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Queries.GetFeed;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetFeedQueryHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheUserIsFollowingAuthorsWithArticles_ReturnsArticleViewModelListOfAuthorsArticles()
        {
            // Arrange
            var getFeedQuery = new GetFeedQuery(null, null);

            // Act
            var handler = new GetFeedQueryHandler(CurrentUserContext, Context, Mapper);
            var response = await handler.Handle(getFeedQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Author.Following.ShouldBeTrue();
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Favorited.ShouldBeTrue();
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.TagList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestDoesNotContainArticlesFromQueryParams_ReturnsEmptyViewModelList()
        {
            // Arrange
            var getFeedQuery = new GetFeedQuery(12, 13);

            // Act
            var handler = new GetFeedQueryHandler(CurrentUserContext, Context, Mapper);
            var response = await handler.Handle(getFeedQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheRequestHasLimitQueryParam_ReturnsLimitedArticleViewModelList()
        {
            // Arrange
            var getFeedQuery = new GetFeedQuery(1, null);

            // Act
            var handler = new GetFeedQueryHandler(CurrentUserContext, Context, Mapper);
            var response = await handler.Handle(getFeedQuery, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Single().Author.Username.ShouldBe("joey.mckenzie");
        }
    }
}