using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class LikedPostConfiguration : IEntityTypeConfiguration<LikedPost>
    {
        public void Configure(EntityTypeBuilder<LikedPost> builder)
        {
            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(f => f.CreatedOn)
                .IsRequired();

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
