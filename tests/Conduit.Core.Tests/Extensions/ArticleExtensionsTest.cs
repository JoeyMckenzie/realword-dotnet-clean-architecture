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
        private readonly IList<ArticleTag> _articleTags;
        private readonly ArticleViewModelList _articleViewModelList;

        public ArticleExtensionsTest()
        {
            _articles = Context.Articles.ToList();
            _articleTags = _articles.SelectMany(a => a.ArticleTags).ToList();
            _articleViewModelList = new ArticleViewModelList
            {
                Articles = Mapper.Map<IEnumerable<ArticleDto>>(_articles)
            };

            _articleViewModelList.ShouldNotBeNull();
            _articleViewModelList.Articles.ShouldNotBeEmpty();
            _articleViewModelList.ArticlesCount.ShouldBe(2);
        }

        [Fact]
        public async Task SetViewModelProperties_WhenFollowerExists_ReturnsWithFollowingSetToTrue()
        {
            // Arrange
            var authorToFollow = _articleViewModelList.Articles.FirstOrDefault(a => a.Author.Username == "joey.mckenzie");
            authorToFollow.ShouldNotBeNull();
            authorToFollow.Author.Following.ShouldBeFalse();
            var user = await CurrentUserContext.GetCurrentUserContext();

            // Act
            _articleViewModelList.SetViewModelProperties(_articles, user, _articleTags);

            // Assert
            authorToFollow.Author.Following.ShouldBeTrue();
            authorToFollow.Favorited.ShouldBeTrue();
            authorToFollow.TagList.ShouldNotBeEmpty();
        }
    }
}