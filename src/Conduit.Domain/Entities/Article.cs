namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;

    public class Article : BaseEntity
    {
        public Article()
        {
            ArticleTags = new HashSet<ArticleTag>();
            Favorites = new HashSet<Favorite>();
            Comments = new HashSet<Comment>();
        }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string AuthorId { get; set; }

        public virtual ConduitUser Author { get; set; }

        public ISet<ArticleTag> ArticleTags { get; }

        public ISet<Favorite> Favorites { get; }

        public ISet<Comment> Comments { get; }

        public int FavoritesCount { get; set; }
    }
}