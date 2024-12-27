namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public interface IGetCategoriesUseCase
{
    Task<GetCategoriesResponse> Handle(GetCategoriesRequest request, CancellationToken ct = default);
}