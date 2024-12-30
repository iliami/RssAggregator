namespace RssAggregator.Application.UseCases.Posts.GetPost;

public record GetPostResponse(
    Guid Id,
    string Title,
    string Description,
    string[] Categories,
    DateTime PublishDate,
    string Url,
    Guid FeedId)
{
    public static GetPostResponse Empty =>
        new GetPostResponse(
            Guid.Empty,
            string.Empty,
            string.Empty,
            Array.Empty<string>(),
            DateTime.UnixEpoch,
            string.Empty,
            Guid.Empty);
}