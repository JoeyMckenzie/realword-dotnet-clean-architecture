namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;

    public class Article : BaseEntity
    {
        public Article()
        {
            ArticleTags = new List<ArticleTag>();
            Favorites = new List<Favorite>();
            Comments = new List<Comment>();
        }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public string AuthorId { get; set; }

        public virtual ConduitUser Author { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; }

        public ICollection<Favorite> Favorites { get; }

        public ICollection<Comment> Comments { get; }

        public int FavoritesCount => Favorites.Count;
    }
}