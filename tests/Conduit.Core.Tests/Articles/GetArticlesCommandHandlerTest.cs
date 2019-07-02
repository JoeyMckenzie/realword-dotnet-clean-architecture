namespace Conduit.Core.Tests.Articles
{
    using System.Collections.Generic;
    using System.Linq;
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
        public async Task GivenValidRequest_WhenRequestContainsPossibleQueryParams_ReturnsArticlesVieWModel()
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
            response.Articles.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsPossibleQueryParamsWithNoResultMatches_ReturnsArticlesVieWModelWithNoResults()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, "joey.mckenzie", "thisUserDoesNotExist", null, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldBeEmpty();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestDoesNotContainQueryParams_ReturnsAllArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, null, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Count().ShouldBe(2);
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsLimitParam_ReturnsLimitedResultsArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, 1, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Count().ShouldBe(1);
            response.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsOffsetParam_ReturnsSkippedResultsArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, null, 1);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Count().ShouldBe(1);
            response.Articles.ShouldContain(a => a.Author.Username == "test.user");
        }
    }
}