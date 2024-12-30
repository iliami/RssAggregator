using FluentValidation;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Categories.CreateCategory;

public class CreateCategoryUseCase(
    ICreateCategoryStorage storage,
    IValidator<CreateCategoryRequest> validator)
    : ICreateCategoryUseCase
{
    public async Task<CreateCategoryResponse> Handle(
        CreateCategoryRequest request,
        CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var isFeedExist = await storage.IsFeedExist(request.FeedId, ct);

        if (!isFeedExist)
        {
            throw new NotFoundException<Feed>(request.FeedId);
        }

        var categoryId = await storage.CreateCategory(
            request.Name, request.FeedId, ct);

        var response = new CreateCategoryResponse(categoryId);

        return response;
    }
}