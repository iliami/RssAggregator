using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Tests.UseCases.Posts.GetPost;

public class GetPostUseCaseShould
{
    private readonly GetPostUseCase _sut;
    private readonly IGetPostStorage _storage;

    public GetPostUseCaseShould()
    {
        var validator = Substitute.For<IValidator<GetPostRequest>>();
        _storage = Substitute.For<IGetPostStorage>();

        _sut = new GetPostUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnEmptyGetPostResponse_WhenPostIsNotFound()
    {
        var postId = Guid.Parse("26060E88-B055-416A-97ED-6CBB5AB8ACF8");

        var request = new GetPostRequest(postId);
        _storage.TryGetAsync(postId, Arg.Any<CancellationToken>()).Returns((false, null));
        var expected = GetPostResponse.Empty;

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ReturnSomeGetPostResponse_WhenPostIsFound()
    {
        var postId = Guid.Parse("6AE7CCA2-3D4C-4C61-B5C7-F1955409C161");
        var post = CreatePost(postId);

        var request = new GetPostRequest(postId);
        _storage.TryGetAsync(postId, Arg.Any<CancellationToken>()).Returns((true, post));
        var expected = new GetPostResponse(
            post.Id,
            post.Title,
            post.Description,
            post.Categories.Select(c => c.Name).ToArray(),
            post.PublishDate,
            post.Url,
            post.Feed.Id);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    private static Post CreatePost(Guid postId) =>
        new()
        {
            Id = postId,
            Title = "Title",
            Description = "Description",
            Url = "Url",
            PublishDate = DateTime.Now,
            Categories = [],
            Feed = new Feed
            {
                Id = Guid.Empty,
                Name = "Name",
                Description = "Description",
                Url = "Url",
                LastFetchedAt = DateTimeOffset.Now,
                Subscribers = []
            }
        };
}