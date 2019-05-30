namespace Conduit.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public virtual string UserId { get; set; }

        public ConduitUser User { get; set; }
    }
}