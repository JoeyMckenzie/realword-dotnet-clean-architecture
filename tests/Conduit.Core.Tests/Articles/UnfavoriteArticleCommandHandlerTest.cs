namespace Conduit.Core.Tests.Articles
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.UnfavoriteArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class UnfavoriteArticleCommandHandlerTest : TestFixture
    {
        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExistsAndTheUserHasFavorited_ReturnsArticleViewModelWithArticleUnfavorited()
        {
            // Arrange
            var unfavoriteCommand = new UnfavoriteArticleCommand("how-to-train-your-dragon");
            var existingFavorite = Context.Favorites.Where(f => f.Article.Slug == "how-to-train-your-dragon").ToList();
            existingFavorite.ShouldNotBeNull();
            existingFavorite.ShouldContain(f => f.User.UserName == "test.user");
            Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon")?.FavoritesCount.ShouldBe(1);

            // Act
            var handler = new UnfavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper);
            var response = await handler.Handle(unfavoriteCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModel>();
            response.Article.ShouldNotBeNull();
            response.Article.ShouldBeOfType<ArticleDto>();
            response.Article.Favorited.ShouldBeFalse();
            Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon")?.FavoritesCount.ShouldBe(0);
            Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon")?.Favorites.ShouldNotContain(f => f.User.UserName == "test.user");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExistsAndTheUserHasNotFavorited_ReturnsArticleViewModelWithArticleUnfavorited()
        {
            // Arrange
            var unfavoriteCommand = new UnfavoriteArticleCommand("why-beer-is-gods-gift-to-the-world");
            var existingFavorite = Context.Favorites.Where(f => f.Article.Slug == "why-beer-is-gods-gift-to-the-world").ToList();
            existingFavorite.ShouldNotBeNull();
            existingFavorite.ShouldNotContain(f => f.User.UserName == "test.user");
            Context.Articles.FirstOrDefault(a => a.Slug == "why-beer-is-gods-gift-to-the-world")?.FavoritesCount.ShouldBe(0);

            // Act
            var handler = new UnfavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper);
            var response = await handler.Handle(unfavoriteCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModel>();
            response.Article.ShouldNotBeNull();
            response.Article.ShouldBeOfType<ArticleDto>();
            response.Article.Favorited.ShouldBeFalse();
            Context.Articles.FirstOrDefault(a => a.Slug == "why-beer-is-gods-gift-to-the-world")?.FavoritesCount.ShouldBe(0);
            Context.Articles.FirstOrDefault(a => a.Slug == "why-beer-is-gods-gift-to-the-world")?.Favorites.ShouldNotContain(f => f.User.UserName == "test.user");
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ThrowsApiExpceptionForNotFound()
        {
            // Arrange
            var unfavoriteCommand = new UnfavoriteArticleCommand("a-girl-does-not-have-a-name");

            // Act
            var handler = new UnfavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(unfavoriteCommand, CancellationToken.None);
            });

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ConduitApiException>();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}