using System.Xml.Serialization;
using RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob.RssXmlModels;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob;

public class RssFeedFetcher(IHttpClientFactory httpClientFactory, string feedUrl)
{
    public async Task<RssFeed> Fetch(CancellationToken ct = default)
    {
        using var httpClient = httpClientFactory.CreateClient();
        var serializer = new XmlSerializer(typeof(RssRoot));
        var xmlString = await httpClient.GetStringAsync(feedUrl, ct);
        using var stringReader = new StringReader(xmlString);

        var feedFromInternet = (RssRoot)serializer.Deserialize(stringReader)!;
        return feedFromInternet.Channel;
    }
}