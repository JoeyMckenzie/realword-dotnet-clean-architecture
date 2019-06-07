namespace Conduit.Domain.Entities
{
    public class ActivityLog : BaseEntity
    {
        public string ActivityType { get; set; }

        public TransactionType TransactionType { get; set; }

        public string TransactionId { get; set; }
    }
}