namespace Conduit.Domain.Entities
{
    public class UserFollow : BaseEntity
    {
        public virtual string UserFollowerId { get; set; }

        public User UserFollower { get; set; }

        public virtual string UserFollowingId { get; set; }

        public User UserFollowing { get; set; }
    }
}