namespace RssAggregator.Presentation.Contracts.Requests.Api;

public record GetUserPostsRequest(int Page = 1, int PageSize = 50);