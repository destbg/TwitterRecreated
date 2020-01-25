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
                .HasColumnType("char(36)");

            builder.Property(f => f.PollEnd)
                .IsRequired(false);

            builder.Property(f => f.PostedOn)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(f => f.Video)
                .HasMaxLength(100)
                .IsRequired(false)
                .HasColumnType("varchar(100)");

            builder.HasOne(f => f.Repost)
                .WithOne(f => f.Post)
                .HasForeignKey<Repost>(b => b.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
