using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class MessageReadConfiguration : IEntityTypeConfiguration<MessageRead>
    {
        public void Configure(EntityTypeBuilder<MessageRead> builder) =>
            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .HasColumnType("char(36)");
    }
}
