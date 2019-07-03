namespace Conduit.Core.Tests.Articles
{
    using System.Collections.Generic;
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
        }
    }
}