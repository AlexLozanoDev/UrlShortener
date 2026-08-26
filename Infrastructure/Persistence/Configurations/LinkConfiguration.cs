using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class LinkConfiguration : IEntityTypeConfiguration<Link>
    {
        public void Configure(EntityTypeBuilder<Link> builder)
        {
            builder.ToTable("Links");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.ShortCode)
                .HasConversion(
                    shortCode => shortCode.Value,
                    code => new ShortCode(code))
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(l => l.ShortCode)
                .IsUnique();

            builder.Property(l => l.TargetUrl)
                .IsRequired()
                .HasMaxLength(2048);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.OwnerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
