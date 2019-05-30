namespace Conduit.Domain.Entities
{
    public class UserFollow : BaseEntity
    {
        public virtual string UserFollowerId { get; set; }

        public ConduitUser UserFollower { get; set; }

        public virtual string UserFollowingId { get; set; }

        public ConduitUser UserFollowing { get; set; }
    }
}