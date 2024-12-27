namespace RssAggregator.Application.UseCases.Categories.CreateCategory;

public interface ICreateCategoryStorage
{
    Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default);
    Task<Guid> CreateCategory(
        string name, Guid feedId,
        CancellationToken ct = default);
}