using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.SyncFeedsService.RssXmlModels;

[XmlRoot("rss")]
public class RssRoot
{
    [XmlElement("channel")] public RssFeed Channel { get; set; } = null!;
}