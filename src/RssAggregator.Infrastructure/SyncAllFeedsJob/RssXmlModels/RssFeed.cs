using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.SyncAllFeedsJob.RssXmlModels;

public class RssFeed
{
    [XmlElement("title")] public string Name { get; set; } = string.Empty;
    [XmlElement("link")] public string Url { get; set; } = string.Empty;
    [XmlElement("description")] public string Description { get; set; } = string.Empty;
    [XmlElement("language")] public string Language { get; set; } = string.Empty;
    [XmlElement("item")] public List<RssItem> Items { get; set; } = null!;
}