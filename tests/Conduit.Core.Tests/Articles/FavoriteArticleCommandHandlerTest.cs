namespace Conduit.Core.Tests.Articles
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Articles.Commands.FavoriteArticle;
    using Domain.Dtos.Articles;
    using Domain.ViewModels;
    using Exceptions;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class FavoriteArticleCommandHandlerTest : TestFixture
    {
        public FavoriteArticleCommandHandlerTest()
        {
            // Reset favorites
            var article = Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon");
            article.ShouldNotBeNull();
            article.Favorites.Clear();
            article.FavoritesCount--;
            Context.SaveChanges();
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleExists_ReturnsFavoritedArticleViewModel()
        {
            // Arrange, remove existing favorite
            var favoriteArticleCommand = new FavoriteArticleCommand("how-to-train-your-dragon");
            var article = Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon");
            article.ShouldNotBeNull();
            article.Favorites.ShouldNotContain(f => f.User.UserName == "test.user");
            article.FavoritesCount.ShouldBe(0);

            // Act
            var handler = new FavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper, MachineDateTime);
            var response = await handler.Handle(favoriteArticleCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModel>();
            response.Article.ShouldNotBeNull();
            response.Article.ShouldBeOfType<ArticleDto>();
            response.Article.Favorited.ShouldBeTrue();
            article.Favorites.ShouldContain(f => f.User.UserName == "test.user");
            article.FavoritesCount.ShouldBe(1);
        }

        [Fact]
        public async Task GivenValidRequest_WhenUsersFavoritesTheArticleMultipleTimes_ReturnsFavoritedArticleViewModelFromPreviousFavorite()
        {
            // Arrange
            var favoriteArticleCommand = new FavoriteArticleCommand("how-to-train-your-dragon");
            var article = Context.Articles.FirstOrDefault(a => a.Slug == "how-to-train-your-dragon");
            article.ShouldNotBeNull();
            article.Favorites.ShouldNotContain(f => f.User.UserName == "test.user");
            article.FavoritesCount.ShouldBe(0);

            // Act
            var handler = new FavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper, MachineDateTime);
            await handler.Handle(favoriteArticleCommand, CancellationToken.None);
            var response = await handler.Handle(favoriteArticleCommand, CancellationToken.None);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ArticleViewModel>();
            response.Article.ShouldNotBeNull();
            response.Article.ShouldBeOfType<ArticleDto>();
            response.Article.Favorited.ShouldBeTrue();
            article.Favorites.ShouldContain(f => f.User.UserName == "test.user");
            article.FavoritesCount.ShouldBe(1);
        }

        [Fact]
        public async Task GivenValidRequest_WhenTheArticleDoesNotExist_ReturnsErrorVieWModelWithNotFound()
        {
            // Arrange
            var favoriteArticleCommand = new FavoriteArticleCommand("how-to-not-train-your-dragon");

            // Act
            var handler = new FavoriteArticleCommandHandler(CurrentUserContext, Context, Mapper, MachineDateTime);
            var response = await Should.ThrowAsync<ConduitApiException>(async () =>
            {
                await handler.Handle(favoriteArticleCommand, CancellationToken.None);
            });

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeOfType<ConduitApiException>();
            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        }
    }
}