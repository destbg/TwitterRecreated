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
                .IsRequired(false)
                .HasColumnType("datetime2");

            builder.Property(f => f.Video)
                .HasMaxLength(100)
                .IsRequired(false)
                .HasColumnType("varchar(100)");

            builder.HasOne(f => f.Repost)
                .WithOne();

            builder.HasOne(f => f.Reply)
                .WithOne();

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
