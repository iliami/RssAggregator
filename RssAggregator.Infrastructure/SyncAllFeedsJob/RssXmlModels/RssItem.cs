using System.Xml.Serialization;

namespace RssAggregator.Infrastructure.SyncAllFeedsJob.RssXmlModels;

public class RssItem
{
    [XmlElement("title")] public string Title { get; set; } = string.Empty;
    [XmlElement("link")] public string Url { get; set; } = string.Empty;
    [XmlElement("description")] public string Description { get; set; } = string.Empty;
    [XmlElement("pubDate")] public string PublishDate { get; set; } = string.Empty;
    [XmlElement("category")] public List<string> Categories { get; set; } = [];
}