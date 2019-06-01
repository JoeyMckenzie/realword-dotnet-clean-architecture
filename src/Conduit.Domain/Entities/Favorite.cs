namespace Conduit.Domain.Entities
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }

        public virtual ConduitUser User { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}