using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class LinkClickStatsConfiguration : IEntityTypeConfiguration<LinkClickStats>
    {
        public void Configure(EntityTypeBuilder<LinkClickStats> builder)
        {
            builder.ToTable("LinkClickStats");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ShortCode)
                .HasConversion(
                    shortCode => shortCode.Value,
                    value => new ShortCode(value)
                )
                .IsRequired();

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45);

            builder.Property(x => x.UserAgent)
                .HasMaxLength(500);
        }
    }
}
