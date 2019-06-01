namespace Conduit.Domain.Entities
{
    using System.Collections.Generic;

    public class Tag : BaseEntity
    {
        public string Description { get; set; }

        public ISet<ArticleTag> ArticleTags { get; set; }
    }
}