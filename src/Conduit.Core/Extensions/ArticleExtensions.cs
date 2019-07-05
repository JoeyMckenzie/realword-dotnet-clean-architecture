namespace Conduit.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

                // Get the associated article tags
                var associatedTags = articleTags?
                    .Where(at => at.ArticleId == mappedArticleEntity?.Id)
                    .Select(at => at.Tag.Description);

                // Set the following and favorited properties
                if (mappedArticleEntity != null)
                {
                    article.Author.Following = mappedArticleEntity.Author.Followers.Any(f => f.UserFollower == user);
                    article.Favorited = mappedArticleEntity.Favorites.Any(f => f.User == user);
                    article.TagList = associatedTags;
                }
            }
        }
    }
}