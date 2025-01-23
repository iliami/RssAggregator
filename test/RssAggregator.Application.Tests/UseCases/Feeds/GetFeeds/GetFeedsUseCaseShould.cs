using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Feeds.GetFeeds;

public class GetFeedsUseCaseShould
{
    private readonly GetFeedsUseCase _sut;
    private readonly IGetFeedsStorage _storage;

    private class TestSpecification : Specification<Feed>;

    public GetFeedsUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetFeedsRequest>>();
        _storage = Substitute.For<IGetFeedsStorage>();

        _sut = new GetFeedsUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponseWithFeeds_WhenAllGood()
    {
        var feeds = GenerateFeeds(10);
        var request = new GetFeedsRequest(new TestSpecification());
        _storage
            .GetFeeds(Arg.Any<Specification<Feed>>(), CancellationToken.None)
            .Returns(feeds);
        var expected = new GetFeedsResponse(feeds);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    private static Feed[] GenerateFeeds(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Feed
            {
                Id = Guid.NewGuid(),
                Name = $"Feed {i}",
                Url = $"https://feeds.com/import/{i}/rss.xml"
            })
            .ToArray();
    }
}