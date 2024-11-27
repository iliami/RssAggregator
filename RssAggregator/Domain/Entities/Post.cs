using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace RssAggregator.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    [StringLength(1024)]
    public required string Title { get; set; }
    [StringLength(32768)]
    public required string Description { get; set; }
    public DateTime PublishDate { get; set; }
    [StringLength(256)]
    public required string Url { get; set; }
    [StringLength(64)]
    public required string Category { get; set; }
    public Guid FeedId { get; set; }
    public Feed Feed { get; set; } = null!;
}