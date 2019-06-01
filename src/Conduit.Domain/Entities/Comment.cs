namespace Conduit.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Body { get; set; }

        public string UserId { get; set; }

        public virtual ConduitUser User { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}