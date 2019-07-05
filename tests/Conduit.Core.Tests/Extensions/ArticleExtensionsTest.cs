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
        private readonly ArticleViewModel _articleViewModel;
        private readonly ArticleViewModelList _articleViewModelList;

        public ArticleExtensionsTest()
        {
            // Get the articles in the test database
            _articles = Context.Articles.ToList();
            _articleTags = _articles.SelectMany(a => a.ArticleTags).ToList();

            // Instantiate and map the view models
            _articleViewModel = new ArticleViewModel
            {
                Article = Mapper.Map<ArticleDto>(_articles.First())
            };

            _articleViewModelList = new ArticleViewModelList
            {
                Articles = Mapper.Map<IEnumerable<ArticleDto>>(_articles)
            };

            // Assert the article structure is correct
            _articleViewModel.ShouldNotBeNull();
            _articleViewModel.Article.ShouldNotBeNull();
            _articleViewModel.Article.Author.Username.ShouldBe("joey.mckenzie");
            _articleViewModelList.ShouldNotBeNull();
            _articleViewModelList.Articles.ShouldNotBeEmpty();
            _articleViewModelList.ArticlesCount.ShouldBe(2);
        }

        [Fact]
        public async Task SetViewModelProperties_ForEachArticle_SetsTheRequiredViewModelProperties()
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

        [Fact]
        public async Task SetViewModelPropertiesForArticle_SetsTheRequiredViewModelProperties()
        {
            // Arrange
            var user = await CurrentUserContext.GetCurrentUserContext();

            // Act
            _articleViewModel.SetViewModelPropertiesForArticle(_articleViewModel.Article, _articles.First(), _articleTags, user);

            // Assert
            _articleViewModel.Article.Author.Following.ShouldBeTrue();
            _articleViewModel.Article.Favorited.ShouldBeTrue();
            _articleViewModel.Article.TagList.ShouldNotBeEmpty();
        }
    }
}