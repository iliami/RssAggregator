using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Configurations;

public class FeedConfiguration : IEntityTypeConfiguration<Feed>
{
    public void Configure(EntityTypeBuilder<Feed> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(128);

        builder.Property(x => x.Url).HasMaxLength(256);

        builder.Property(x => x.Description).HasMaxLength(2048);

        builder.HasMany(x => x.Subscribers).WithMany()
            .UsingEntity("Subscriptions",
                r => r.HasOne(typeof(User)).WithMany().HasForeignKey("AppUserId")
                    .HasPrincipalKey(nameof(User.Id)),
                l => l.HasOne(typeof(Feed), nameof(Feed.Subscribers)).WithMany().HasForeignKey("FeedId")
                    .HasPrincipalKey(nameof(Feed.Id)),
                j => j.HasKey("FeedId", "AppUserId"));

        builder.HasMany(x => x.Posts).WithOne(x => x.Feed);
    }
}