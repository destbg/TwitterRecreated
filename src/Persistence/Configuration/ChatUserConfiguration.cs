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
                .IsUnicode(false);

            builder.Property(f => f.OthersColor)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6)
                .IsUnicode(false);

            builder.Property(f => f.SelfColor)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6)
                .IsUnicode(false);

            builder.Property(f => f.CreatedOn)
                .IsRequired();

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasOne(f => f.MessageRead)
                .WithMany(f => f.ChatUsers);
        }
    }
}
