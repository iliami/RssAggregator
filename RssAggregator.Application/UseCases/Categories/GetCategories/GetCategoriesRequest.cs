using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public record GetCategoriesRequest(Specification<Category> Specification);