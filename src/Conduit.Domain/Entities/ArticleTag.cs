namespace Conduit.Domain.Entities
{
    public class ArticleTag : BaseEntity
    {
        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}