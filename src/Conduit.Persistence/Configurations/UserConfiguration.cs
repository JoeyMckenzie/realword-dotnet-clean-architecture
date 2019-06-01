namespace Conduit.Persistence.Configurations
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserConfiguration : IEntityTypeConfiguration<ConduitUser>
    {
        public void Configure(EntityTypeBuilder<ConduitUser> builder)
        {
            builder.HasMany(cu => cu.Articles)
                .WithOne(a => a.Author)
                .HasForeignKey(cu => cu.AuthorId);

            builder.Property(cu => cu.Bio)
                .HasDefaultValue();

            builder.Property(cu => cu.Email)
                .IsRequired();
        }
    }
}