using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.Posts.GetPosts;

public class GetPostsUseCaseShould
{
    private readonly GetPostsUseCase _sut;
    private readonly IGetPostsStorage _storage;

    private class TestSpecification : Specification<Post>;
    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("47B2B9A2-2562-48F9-866A-29868FD1E3E8"),
        Name = "Feed",
        Description = "Some description",
        Url = "https://example.com",
        LastFetchedAt = DateTimeOffset.UtcNow.AddHours(-1)
    };

    public GetPostsUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetPostsRequest>>();
        _storage = Substitute.For<IGetPostsStorage>();

        _sut = new GetPostsUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponseWithoutPosts_WhenNoPosts()
    {
        var request = new GetPostsRequest(new TestSpecification());
        _storage
            .GetPosts(
                Arg.Any<Specification<Post>>(),
                CancellationToken.None)
            .Returns([]);
        var expected = new GetPostsResponse([]);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).GetPosts(
            Arg.Any<Specification<Post>>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnResponseWithPosts_WhenRequestIsValid()
    {
        var posts = GeneratePosts(20);
        var request = new GetPostsRequest(new TestSpecification());
        _storage
            .GetPosts(
                Arg.Any<Specification<Post>>(),
                CancellationToken.None)
            .Returns(posts);
        var expected = new GetPostsResponse(posts);

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).GetPosts(
            Arg.Any<Specification<Post>>(),
            Arg.Any<CancellationToken>());
    }

    private static Post[] GeneratePosts(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new Post
            {
                Id = Guid.NewGuid(),
                Title = $"Post {i}",
                Categories = [],
                PublishDate = DateTime.UtcNow,
                Description = $"Content {i}",
                Feed = Feed,
                Url = Feed.Url + $"/{i}"
            })
            .ToArray();
    }
}