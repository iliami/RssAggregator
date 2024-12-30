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

        builder.HasOne(x => x.Feed).WithMany(x => x.Posts);

        builder.HasMany(x => x.Categories).WithMany()
            .UsingEntity(x =>
            {
                x.ToTable("PostCategory");
                x.Property("CategoriesId").Metadata.SetColumnName("CategoryId");
            });
    }
}