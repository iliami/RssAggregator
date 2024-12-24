using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.BackgroundJobs.SyncAllFeedsJob.RssXmlModels;

public class RssFeed
{
    [XmlElement("title")] public string Title { get; set; } = string.Empty;
    [XmlElement("link")] public string Link { get; set; } = string.Empty;
    [XmlElement("description")] public string Description { get; set; } = string.Empty;
    [XmlElement("language")] public string Language { get; set; } = string.Empty;
    [XmlElement("item")] public List<RssItem> Items { get; set; } = null!;
}