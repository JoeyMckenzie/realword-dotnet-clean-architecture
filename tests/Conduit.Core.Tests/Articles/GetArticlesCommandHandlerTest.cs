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
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper, CurrentUserContext);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.FirstOrDefault()?.Author.Following.ShouldBeTrue();
            response.Articles.FirstOrDefault()?.Favorited.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsPossibleQueryParamsWithNoResultMatches_ReturnsArticlesVieWModelWithNoResults()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, "joey.mckenzie", "thisUserDoesNotExist", null, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper, CurrentUserContext);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldBeEmpty();
            response.Articles.FirstOrDefault()?.Author.Following.ShouldBeTrue();
            response.Articles.FirstOrDefault()?.Favorited.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestDoesNotContainQueryParams_ReturnsAllArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, null, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper, CurrentUserContext);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.ArticlesCount.ShouldBe(2);
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Author.Following.ShouldBeTrue();
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Favorited.ShouldBeTrue();
            response.Articles.FirstOrDefault(a => a.Author.Username == "test.user")?.Author.Following.ShouldBeFalse();
            response.Articles.FirstOrDefault(a => a.Author.Username == "test.user")?.Favorited.ShouldBeFalse();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsLimitParam_ReturnsLimitedResultsArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, 1, null);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper, CurrentUserContext);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Count().ShouldBe(1);
            response.Articles.ShouldContain(a => a.Author.Username == "joey.mckenzie");
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Author.Following.ShouldBeTrue();
            response.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie")?.Favorited.ShouldBeTrue();
        }

        [Fact]
        public async Task GivenValidRequest_WhenRequestContainsOffsetParam_ReturnsSkippedResultsArticlesInArticlesVieWModel()
        {
            // Arrange
            var getArticlesCommand = new GetArticlesQuery(null, null, null, null, 1);

            // Act
            var command = new GetArticlesQueryHandler(UserManager, Context, Mapper, CurrentUserContext);
            var response = await command.Handle(getArticlesCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModelList>();
            response.Articles.ShouldNotBeNull();
            response.Articles.ShouldBeOfType<List<ArticleDto>>();
            response.Articles.ShouldNotBeEmpty();
            response.Articles.Count().ShouldBe(1);
            response.Articles.ShouldContain(a => a.Author.Username == "test.user");
            response.Articles.FirstOrDefault(a => a.Author.Username == "test.user")?.Author.Following.ShouldBeFalse();
            response.Articles.FirstOrDefault(a => a.Author.Username == "test.user")?.Favorited.ShouldBeFalse();
        }
    }
}