namespace Conduit.Core.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities;
    using Domain.ViewModels;

    public static class ArticleExtensions
    {
        public static void SetFollowingAndFavorited(this ArticleViewModelList articleViewModelList, IList<Article> articles, ConduitUser user)
        {
            foreach (var article in articleViewModelList.Articles)
            {
                // Retrieve the corresponding article
                var mappedArticleEntity = articles.FirstOrDefault(a => string.Equals(a.Title, article.Title, StringComparison.OrdinalIgnoreCase));

                // Set the following and favorited properties
                if (mappedArticleEntity != null)
                {
                    article.Author.Following = mappedArticleEntity.Author.Followers.Any(f => f.UserFollower == user);
                    article.Favorited = mappedArticleEntity.Favorites.Any(f => f.User == user);
                }
            }
        }
    }
}