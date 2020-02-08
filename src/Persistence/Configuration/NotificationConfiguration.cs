using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(f => f.ForUserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(f => f.NotificationType)
                .IsRequired();

            builder.Property(f => f.PostId)
                .IsRequired(false);

            builder.Property(f => f.CreatedOn)
                .IsRequired();

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
