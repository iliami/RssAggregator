using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncFeedsService.RssXmlModels;

public class RssItem
{
    [XmlElement("title")] public string Title { get; set; } = string.Empty;
    [XmlElement("link")] public string Link { get; set; } = string.Empty;
    [XmlElement("description")] public string Description { get; set; } = string.Empty;
    [XmlElement("pubDate")] public string PubDate { get; set; } = string.Empty;
    [XmlElement("category")] public string Category { get; set; } = string.Empty;
}