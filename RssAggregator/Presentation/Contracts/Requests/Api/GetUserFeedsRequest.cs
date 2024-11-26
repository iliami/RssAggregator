namespace RssAggregator.Presentation.Contracts.Requests.Api;

public record GetUserFeedsRequest(int Page = 1, int PageSize = 10);