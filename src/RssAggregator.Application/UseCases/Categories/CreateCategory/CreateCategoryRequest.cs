namespace RssAggregator.Application.UseCases.Categories.CreateCategory;

public record CreateCategoryRequest(string Name, Guid FeedId);