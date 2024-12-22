using FluentAssertions;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.Posts.AddPostsInFeed;

public class AddPostsRequestValidatorShould
{
    private readonly AddPostsInFeedRequestValidator _sut;

    private static readonly Feed Feed = new Feed
    {
        Id = Guid.Parse("F6BB1027-5435-4E25-9AAA-8CCE6AAD0A6C"),
        Name = "Test feed",
        Description = "Test feed description",
        Url = "https://www.example.com",
        LastFetchedAt = DateTimeOffset.UtcNow
    };

    public AddPostsRequestValidatorShould()
    {
        _sut = new AddPostsInFeedRequestValidator();
    }

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var posts = GenerateValidPostsForFeed(5, Feed);
        var request = new AddPostsInFeedRequest(posts, Feed);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public void ReturnFailure_WhenRequestIsInvalid(AddPostsInFeedRequest request)
    {
        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }

    private static Post[] GenerateValidPostsForFeed(int count, Feed feed)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Post {i}",
                Description = $"Content {i}",
                Url = $"posts/{i}",
                PublishDate = DateTime.UtcNow.AddHours(-i),
                Categories = [],
                Feed = feed,
            })
            .ToArray();
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        var otherFeed = new Feed
        {
            Id = Guid.Parse("50896DAF-C310-4492-8578-B5CB974419C1"),
            Name = "Other feed for test",
            Description = "Other feed for test description",
            Url = "https://www.other.example.com",
            LastFetchedAt = DateTimeOffset.UtcNow
        };
        var posts = GenerateValidPostsForFeed(5, Feed);
        var request = new AddPostsInFeedRequest(posts, Feed);

        yield return [request with { Feed = null! }];
        yield return [request with { Feed = otherFeed }];
        yield return [request with { Posts = null! }];
        yield return [request with { Posts = [] }];
        yield return [request with { Posts = posts.Select((p, i) =>
        {
            if (i == 0)
            {
                p.Feed = otherFeed;
            } 
            return p;
        }).ToArray() }];
        yield return [request with { Posts = posts.Select((p, i) => i == 0 ? null! : p).ToArray() }];
    }
}