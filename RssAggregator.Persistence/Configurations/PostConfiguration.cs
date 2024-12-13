using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(1024);

        builder.Property(x => x.Description).HasMaxLength(32768);

        builder.Property(x => x.Url).HasMaxLength(256);

        builder.Property(x => x.Category).HasMaxLength(64);

        builder.HasOne(x => x.Feed).WithMany(x => x.Posts);
    }
}