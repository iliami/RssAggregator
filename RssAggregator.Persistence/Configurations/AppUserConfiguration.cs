using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Email).HasMaxLength(32);

        builder.Property(x => x.Password).HasMaxLength(128);

        builder.Property(x => x.Role).HasMaxLength(16);

        builder.HasIndex(x => x.Email).IsUnique();
    }
}