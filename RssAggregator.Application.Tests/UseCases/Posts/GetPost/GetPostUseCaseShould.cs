using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

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
    public async Task ReturnResponseWithPost_WhenPostIsFound()
    {
        var postId = Guid.Parse("6AE7CCA2-3D4C-4C61-B5C7-F1955409C161");
        var post = new Post
        {
            Id = postId,
            Title = "Post name",
            Description = "Post description",
            Url = $"https://www.example.com/posts/{postId}",
            PublishDate = DateTime.Now,
            Categories = [],
            Feed = new Feed
            {
                Id = Guid.Parse("18E5BDB4-D630-4481-84FF-AA435ED7BF3F"),
                Name = "Feed name",
                Description = "Feed description",
                Url = "https://www.example.com/",
                LastFetchedAt = DateTimeOffset.Now
            }
        };

        var request = new GetPostRequest(postId);
        _storage.TryGetPost(postId, Arg.Any<CancellationToken>()).Returns((true, post));
        var expected = new GetPostResponse(post);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowPostNotFoundException_WhenPostIsNotFound()
    {
        var postId = Guid.Parse("26060E88-B055-416A-97ED-6CBB5AB8ACF8");
        var request = new GetPostRequest(postId);

        _storage.TryGetPost(Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns((false, null!));

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<PostNotFoundException>();
    }
}