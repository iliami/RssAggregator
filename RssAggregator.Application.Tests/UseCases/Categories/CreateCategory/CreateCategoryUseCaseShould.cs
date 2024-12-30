using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Categories.CreateCategory;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Categories.CreateCategory;

public class CreateCategoryUseCaseShould
{
    private readonly CreateCategoryUseCase _sut;
    private readonly ICreateCategoryStorage _storage;

    public CreateCategoryUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreateCategoryRequest>>();
        _storage = Substitute.For<ICreateCategoryStorage>();

        _sut = new CreateCategoryUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnNotEmptyId_WhenCategoryIsCreated()
    {
        var categoryId = Guid.Parse("BA4FC2A5-289F-4ED3-AC95-A5A1D283867E");
        var feedId = Guid.Parse("E2326A46-E423-4DE4-AB4A-08A72C6FDEA0");
        var request = new CreateCategoryRequest("Category Name", feedId);
        _storage.IsFeedExist(Arg.Any<Guid>(), CancellationToken.None).Returns(true);
        _storage.CreateCategory(Arg.Any<string>(), Arg.Any<Guid>(), CancellationToken.None).Returns(categoryId);
        var expected = new CreateCategoryResponse(categoryId);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowNotFoundException_WhenFeedNotFound()
    {
        var feedId = Guid.Parse("E2326A46-E423-4DE4-AB4A-08A72C6FDEA0");
        var request = new CreateCategoryRequest("Category Name", feedId);
        _storage.IsFeedExist(Arg.Any<Guid>(), CancellationToken.None).Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<NotFoundException<Feed>>();
        await _storage.Received(0).CreateCategory("Category Name", feedId, CancellationToken.None);
    }
}