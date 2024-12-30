using RssAggregator.Application.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public interface IGetCategoriesStorage
{
    Task<Category[]> GetCategories(Specification<Category> specification, CancellationToken ct = default);
}