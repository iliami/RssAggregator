using RssAggregator.Application.Models.Params;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.QueryExtensions;

public static class PostQueryExtensions
{
    public static IQueryable<Post> WithFiltration(this IQueryable<Post> query, PostFilterParams? filterParams = null)
    {
        if (filterParams?.Categories is null || filterParams.Categories.Length == 0)
        {
            return query;
        }

        return filterParams.Categories.Select(c => c.ToLowerInvariant()).Aggregate(
            query,
            (current, category) => current
                .Where(p => p.Categories.Any(c => c.NormalizedName == category)));
    }
}