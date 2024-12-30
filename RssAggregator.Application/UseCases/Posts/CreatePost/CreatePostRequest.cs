namespace RssAggregator.Application.UseCases.Posts.CreatePost;

public record CreatePostRequest(
    string Title,
    string Description,
    string[] Categories,
    DateTime PublishDate,
    string Url,
    Guid FeedId);