using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(64);

        builder.Property(x => x.NormalizedName).HasMaxLength(64);

        builder.HasIndex(x => x.NormalizedName).IsUnique();

        builder.HasOne(c => c.Feed).WithMany();
    }
}