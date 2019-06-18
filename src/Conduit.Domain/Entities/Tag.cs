namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;

    public class Tag : BaseEntity
    {
        public Tag()
        {
            ArticleTags = new HashSet<ArticleTag>();
        }

        public string Description { get; set; }

        public ISet<ArticleTag> ArticleTags { get; }
    }
}