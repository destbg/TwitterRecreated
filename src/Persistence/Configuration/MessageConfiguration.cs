using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(f => f.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(f => f.EditedAt)
                .IsRequired(false);

            builder.Property(f => f.Msg)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .HasColumnType("char(36)");
        }
    }
}
