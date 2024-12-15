namespace RssAggregator.Domain.Entities;

public class Post
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime PublishDate { get; set; }
    public required string Url { get; set; }
    public required ICollection<Category> Categories { get; set; } = [];
    public required Feed Feed { get; set; }
}