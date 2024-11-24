using Newtonsoft.Json;

namespace RssAggregator.Presentation.Contracts.Requests.Api;

public record GetFeedPostsRequest(Guid FeedId, int Page = 1, int PageSize = 50);