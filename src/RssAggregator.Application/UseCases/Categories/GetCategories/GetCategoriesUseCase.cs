using FluentValidation;

namespace RssAggregator.Application.UseCases.Categories.GetCategories;

public class GetCategoriesUseCase(IGetCategoriesStorage storage, IValidator<GetCategoriesRequest> validator)
    : IGetCategoriesUseCase
{
    public async Task<GetCategoriesResponse> Handle(GetCategoriesRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var categories = await storage.GetCategories(request.Specification, ct);

        var response = new GetCategoriesResponse(categories);

        return response;
    }
}