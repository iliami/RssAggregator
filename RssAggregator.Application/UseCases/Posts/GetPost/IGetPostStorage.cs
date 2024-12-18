using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public interface IGetPostStorage
{
    Task<(bool success, Post? post)> TryGetAsync(Guid id, CancellationToken ct = default);
}