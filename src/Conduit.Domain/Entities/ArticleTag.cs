namespace Conduit.Domain.Entities
{
    public class ArticleTag : BaseEntity
    {
        public int Id { get; set; }

        public virtual int ArticleId { get; set; }

        public Article Article { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}