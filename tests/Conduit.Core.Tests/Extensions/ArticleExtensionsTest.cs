namespace Conduit.Core.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Extensions;
    using Domain.Dtos.Articles;
    using Domain.Entities;
    using Domain.ViewModels;
    using Infrastructure;
    using Shouldly;
    using Xunit;

    public class ArticleExtensionsTest : TestFixture
    {
        private readonly IList<Article> _articles;
        private readonly ArticleViewModelList _articleViewModelList;

        public ArticleExtensionsTest()
        {
            _articles = Context.Articles.ToList();
            _articleViewModelList = new ArticleViewModelList
            {
                Articles = Mapper.Map<IEnumerable<ArticleDto>>(_articles)
            };

            _articleViewModelList.ShouldNotBeNull();
            _articleViewModelList.Articles.ShouldNotBeEmpty();
            _articleViewModelList.ArticlesCount.ShouldBe(2);
        }

        [Fact]
        public async Task SetFollowingAndFavorited_WhenFollowerExists_ReturnsWithFollowingSetToTrue()
        {
            // Arrange
            var authorToFollow = _articleViewModelList.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie");
            authorToFollow.ShouldNotBeNull();
            authorToFollow.Author.Following.ShouldBeFalse();
            var user = await CurrentUserContext.GetCurrentUserContext();

            // Act
            _articleViewModelList.SetFollowingAndFavorited(_articles, user);

            // Assert
            authorToFollow.Author.Following.ShouldBeTrue();
        }

        [Fact]
        public async Task SetFollowingAndFavorited_WhenArticleIsFavoried_ReturnsWithFavoritedSetToTrue()
        {
            // Arrange
            var authorToFollow = _articleViewModelList.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie");
            authorToFollow.ShouldNotBeNull();
            authorToFollow.Favorited.ShouldBeFalse();
            var user = await CurrentUserContext.GetCurrentUserContext();

            // Act
            _articleViewModelList.SetFollowingAndFavorited(_articles, user);

            // Assert
            authorToFollow.Favorited.ShouldBeTrue();
        }
    }
}