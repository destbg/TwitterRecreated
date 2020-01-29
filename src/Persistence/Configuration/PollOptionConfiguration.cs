using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class PollOptionConfiguration : IEntityTypeConfiguration<PollOption>
    {
        public void Configure(EntityTypeBuilder<PollOption> builder)
        {
            builder.Property(f => f.Option)
                .HasMaxLength(25)
                .IsRequired();

            builder.HasOne(f => f.Post)
                .WithMany(f => f.Poll)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
