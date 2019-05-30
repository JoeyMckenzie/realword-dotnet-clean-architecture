namespace Conduit.Domain.Entities
{
    public class ArticleTag
    {
        public virtual int ArticleId { get; set; }

        public Article Article { get; set; }

        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}