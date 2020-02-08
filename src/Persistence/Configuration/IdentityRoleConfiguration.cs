using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.Property(f => f.ConcurrencyStamp)
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(f => f.Id)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(36)
                .IsUnicode(false);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(f => f.NormalizedName)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
