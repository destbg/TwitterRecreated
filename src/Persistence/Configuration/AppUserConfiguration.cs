using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .HasColumnType("char(36)");

            builder.Property(f => f.UserName)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("varchar(32)");

            builder.Property(f => f.NormalizedUserName)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("varchar(32)");

            builder.Property(f => f.Email)
                .IsRequired();

            builder.Property(f => f.NormalizedEmail)
                .IsRequired();

            builder.Property(f => f.PasswordHash)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(f => f.ConcurrencyStamp)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(f => f.PhoneNumber)
                .HasMaxLength(256)
                .HasColumnType("varchar(256)");

            builder.Property(f => f.SecurityStamp)
                .HasMaxLength(256)
                .IsRequired()
                .HasColumnType("varchar(256)");

            builder.Property(f => f.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(f => f.DisplayName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(f => f.Image)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("varchar(100)");

            builder.Property(f => f.JoinedOn)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(f => f.LastLogin)
                .IsRequired();

            builder.Property(f => f.Thumbnail)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnType("varchar(100)");
        }
    }
}
