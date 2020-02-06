using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(f => f.Content)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(f => f.UserId)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(f => f.PollEnd)
                .IsRequired(false)
                .HasColumnType("datetime2");

            builder.Property(f => f.Video)
                .HasMaxLength(100)
                .IsRequired(false)
                .IsUnicode(false);

            builder.HasOne(f => f.Repost)
                .WithOne();

            builder.HasOne(f => f.Reply)
                .WithOne();

            builder.Property(f => f.CreatedOn)
                .IsRequired();

            builder.Property(f => f.CreatedByIp)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);
        }
    }
}
