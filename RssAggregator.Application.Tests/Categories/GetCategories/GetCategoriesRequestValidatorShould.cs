using FluentAssertions;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Categories.GetCategories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.Categories.GetCategories;

public class GetCategoriesRequestValidatorShould
{
    private class TestSpecification : Specification<Category>;
    
    private readonly GetCategoriesRequestValidator _sut = new();
    
    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var specification = new TestSpecification();
        var request = new GetCategoriesRequest(specification);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(GetCategoriesRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [new GetCategoriesRequest(null!) ];
    }
}