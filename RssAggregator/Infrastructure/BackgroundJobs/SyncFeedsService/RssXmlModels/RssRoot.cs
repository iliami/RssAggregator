using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService.RssXmlModels;

[XmlRoot("rss")]
public class RssRoot
{
    [XmlElement("channel")] public RssFeed Channel { get; set; } = null!;
}