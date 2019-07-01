namespace Conduit.Core.Tests.Articles
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Queries.GetArticles;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class GetArticlesCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsAllPossibleQueryParams_ReturnsArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery("dragons", "joey.mckenzie", null, null, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
        }
    }
}