namespace Conduit.Domain.Entities
{
    public class UserFollow : BaseEntity
    {
        public string UserFollowerId { get; set; }

        public virtual ConduitUser UserFollower { get; set; }

        public string UserFollowingId { get; set; }

        public virtual ConduitUser UserFollowing { get; set; }
    }
}