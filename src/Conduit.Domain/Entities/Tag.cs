namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;

    public class Tag : BaseEntity
    {
        public Tag()
        {
            ArticleTags = new List<ArticleTag>();
        }

        public string Description { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; }
    }
}