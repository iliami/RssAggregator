using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetCategoriesStorage(AppDbContext dbContext) : IGetCategoriesStorage
{
    public Task<Category[]> GetCategories(Specification<Category> specification, CancellationToken ct = default)
        => dbContext.Categories
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}