namespace Conduit.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.HasOne(uf => uf.UserFollower)
                .WithMany(uf => uf.Following)
                .HasForeignKey(uf => uf.UserFollowerId);

            builder.HasOne(uf => uf.UserFollowing)
                .WithMany(uf => uf.Followers)
                .HasForeignKey(uf => uf.UserFollowingId);
        }
    }
}