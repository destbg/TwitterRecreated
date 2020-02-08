using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class HashTagConfiguration : IEntityTypeConfiguration<HashTag>
    {
        public void Configure(EntityTypeBuilder<HashTag> builder)
        {
            builder.Property(f => f.Country)
                .IsFixedLength()
                .HasMaxLength(2)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(f => f.Tag)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.CreatedOn)
                .IsRequired();

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
