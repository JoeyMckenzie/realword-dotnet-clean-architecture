namespace Conduit.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Dtos.Articles;
    using Domain.Entities;
    using Domain.ViewModels;

    public static class ArticleExtensions
    {
        public static void SetViewModelProperties(this ArticleViewModelList articleViewModelList, IList<Article> articles, ConduitUser user, IList<ArticleTag> articleTags)
        {
            foreach (var article in articleViewModelList.Articles)
            {
                // Retrieve the corresponding article
                var mappedArticleEntity = articles.FirstOrDefault(a => string.Equals(a.Title, article.Title, StringComparison.OrdinalIgnoreCase));
                SetFavoritedFollowingAndArticleTags(article, mappedArticleEntity, user, articleTags);
            }
        }

        public static void SetViewModelPropertiesForArticle(this ArticleViewModel articleViewModel, ArticleDto article, Article articleEntity, IList<ArticleTag> articleTags, ConduitUser user)
        {
            SetFavoritedFollowingAndArticleTags(article, articleEntity, user, articleTags);
        }

        private static void SetFavoritedFollowingAndArticleTags(ArticleDto article, Article articleEntity, ConduitUser user, IList<ArticleTag> articleTags)
        {
            // Get the associated article tags
            var associatedTags = articleTags?
                .Where(at => at.ArticleId == articleEntity?.Id)
                .Select(at => at.Tag.Description);

            // Set the following and favorited properties
            if (articleEntity != null && user != null)
            {
                article.Author.Following = articleEntity.Author.Followers.Any(f => f.UserFollower == user);
                article.Favorited = articleEntity.Favorites.Any(f => f.User == user);
                article.TagList = associatedTags;
            }
        }
    }
}