using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .HasColumnType("char(36)");

            builder.Property(f => f.OthersColor)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6)
                .HasColumnType("char(6)");

            builder.Property(f => f.SelfColor)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6)
                .HasColumnType("char(6)");
        }
    }
}
