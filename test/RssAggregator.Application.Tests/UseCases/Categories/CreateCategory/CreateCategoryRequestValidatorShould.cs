using FluentAssertions;
using RssAggregator.Application.UseCases.Categories.CreateCategory;

namespace RssAggregator.Application.Tests.UseCases.Categories.CreateCategory;

public class CreateCategoryRequestValidatorShould
{
    private readonly CreateCategoryRequestValidator _sut = new();

    private static readonly CreateCategoryRequest ValidRequest = new(
        "Category Name",
        Guid.Parse("BBAA9BCD-C9DF-41CE-A51E-00E25D7CAE63"));

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var actual = _sut.Validate(ValidRequest);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(CreateCategoryRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return [ValidRequest with { Name = null! }];
        yield return [ValidRequest with { Name = string.Empty }];
        yield return [ValidRequest with { FeedId = Guid.Empty }];
    }
}