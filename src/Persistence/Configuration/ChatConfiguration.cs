using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(f => f.Image)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(f => f.CreatedOn)
                .HasColumnType("datetime");

            builder.Property(f => f.Name)
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

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
