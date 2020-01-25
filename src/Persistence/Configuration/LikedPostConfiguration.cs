using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class LikedPostConfiguration : IEntityTypeConfiguration<LikedPost>
    {
        public void Configure(EntityTypeBuilder<LikedPost> builder) =>
            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .HasColumnType("char(36)");
    }
}
