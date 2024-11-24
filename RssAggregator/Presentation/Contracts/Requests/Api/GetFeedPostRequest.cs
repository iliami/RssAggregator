using System.Text.Json.Serialization;

namespace RssAggregator.Presentation.Contracts.Requests.Api;

public record GetFeedPostRequest(Guid FeedId);