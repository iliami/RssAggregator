using RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob.RssXmlModels;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob;

public record ScrapedPostInfo(string Title, string Description, string[] Categories, DateTime PublishDate, string Url);

public record RssFeedProcessorResponse(ScrapedPostInfo[] ScrapedPostsInfos);

public class RssFeedProcessor(RssFeed rssFeed)
{
    public RssFeedProcessorResponse Process()
    {
        var posts = rssFeed.Items
            .Select(scrapedPost =>
            {
                var scrapedPostPublishDate = DateTime
                    .Parse(scrapedPost.PubDate)
                    .ToUniversalTime();

                return new ScrapedPostInfo(
                    scrapedPost.Title, 
                    scrapedPost.Description, 
                    scrapedPost.Categories.ToArray(),
                    scrapedPostPublishDate, 
                    scrapedPost.Link);
            }).ToArray();
        
        var response = new RssFeedProcessorResponse(posts);
        
        return response;
    }
}