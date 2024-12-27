namespace RssAggregator.Application.UseCases.Categories.CreateCategory;

public interface ICreateCategoryUseCase
{
    Task<CreateCategoryResponse> Handle(CreateCategoryRequest request, CancellationToken ct =default);
}