﻿using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Categories.GetCategories;

public class GetCategoriesUseCaseShould
{
    private readonly GetCategoriesUseCase _sut;
    private readonly IGetCategoriesStorage _storage;

    private class TestSpecification : Specification<Category>;

    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("0F270576-6104-46B3-8B61-CCAA2EC1EECD"),
        Name = "Name",
        Description = "Description",
        Url = "www.example.com",
        LastFetchedAt = DateTimeOffset.UtcNow
    };

    public GetCategoriesUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetCategoriesRequest>>();
        _storage = Substitute.For<IGetCategoriesStorage>();

        _sut = new GetCategoriesUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponseWithCategories_WhenAllGood()
    {
        var categories = GenerateCategories(20);
        var request = new GetCategoriesRequest(new TestSpecification());
        _storage
            .GetCategories(
                Arg.Any<Specification<Category>>(),
                CancellationToken.None)
            .Returns(categories);
        var expected = new GetCategoriesResponse(categories);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).GetCategories(
            Arg.Any<Specification<Category>>(),
            Arg.Any<CancellationToken>());
    }

    private static Category[] GenerateCategories(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Category
            {
                Id = Guid.NewGuid(),
                Name = $"Category {i}",
                NormalizedName = $"Category {i}".ToLowerInvariant(),
                Feed = Feed
            })
            .ToArray();
    }
}