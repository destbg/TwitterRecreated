using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollow>
    {
        public void Configure(EntityTypeBuilder<UserFollow> builder)
        {
            builder.Property(f => f.FollowerId)
               .IsRequired()
               .IsFixedLength()
               .HasMaxLength(36)
               .HasColumnType("char(36)");

            builder.Property(f => f.FollowingId)
               .IsRequired()
               .IsFixedLength()
               .HasMaxLength(36)
               .HasColumnType("char(36)");

            builder.Property(f => f.CreatedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("varchar(15)");
        }
    }
}
