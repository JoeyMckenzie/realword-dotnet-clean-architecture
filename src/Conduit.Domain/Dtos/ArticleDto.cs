namespace Conduit.Domain.Dtos
{
    using System.Collections.Generic;

    public class ArticleDto
    {
        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Body { get; set; }

        public ICollection<string> TagList { get; set; }

        public bool Favorited { get; set; }

        public bool FavoritesCount { get; set; }

        public AuthorDto Author { get; set; }
    }
}