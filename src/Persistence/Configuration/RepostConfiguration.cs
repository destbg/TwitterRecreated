using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class RepostConfiguration : IEntityTypeConfiguration<Repost>
    {
        public void Configure(EntityTypeBuilder<Repost> builder)
        {
            builder.Property(f => f.Content)
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(f => f.UserId)
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
