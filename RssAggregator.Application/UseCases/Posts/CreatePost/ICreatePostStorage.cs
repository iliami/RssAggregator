namespace RssAggregator.Application.UseCases.Posts.CreatePost;

public interface ICreatePostStorage
{
    Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default);

    Task<Guid> CreatePost(
        string title,
        string description,
        string[] categories,
        DateTime publishDate,
        string url,
        Guid feedId,
        CancellationToken ct = default);
}