using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Persistence.Entities;

public class Post
{
    public Guid Id { get; set; }
    [StringLength(128)]
    public required string Title { get; set; }
    [StringLength(8192)]
    public required string Content { get; set; }
    public DateTime PublishDate { get; set; }
    [StringLength(256)]
    public required string Url { get; set; }
    
    public Guid FeedId { get; set; }
    public required Feed Feed { get; set; }
}