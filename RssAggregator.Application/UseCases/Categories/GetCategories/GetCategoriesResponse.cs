using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public record GetCategoriesResponse(Category[] Categories);