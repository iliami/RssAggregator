using FluentAssertions;
using FluentValidation;
using NSubstitute;
using RssAggregator.Application.UseCases.Posts.CreatePost;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.Tests.UseCases.Posts.CreatePost;

public class CreatePostUseCaseShould
{
    private readonly CreatePostUseCase _sut;
    private readonly ICreatePostStorage _storage;

    private static readonly Feed Feed = new()
    {
        Id = Guid.Parse("CD0A2A49-6FA3-49D5-BA8E-622F272E891A"),
        Name = "Feed name",
        Description = "Feed description",
        Url = "https://www.example.com",
        LastFetchedAt = DateTimeOffset.UtcNow
    };

    public CreatePostUseCaseShould()
    {
        var validator = Substitute.For<IValidator<CreatePostRequest>>();
        _storage = Substitute.For<ICreatePostStorage>();

        _sut = new CreatePostUseCase(_storage, validator);
    }

    [Fact]
    public async Task ReturnResponseWithCreatedPostId_WhenPostIsCreated()
    {
        var postId = Guid.Parse("0848A88D-270A-4F36-9EEB-CE1855918243");
        var request = new CreatePostRequest(
            "Title",
            "Description",
            ["Category"],
            DateTime.UtcNow.AddDays(-1),
            $"https://www.example.com/posts/{postId}",
            Feed.Id);
        _storage.IsFeedExist(Arg.Any<Guid>(), CancellationToken.None).Returns(true);
        _storage
            .CreatePost(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string[]>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Any<Guid>(),
                CancellationToken.None)
            .Returns(postId);
        var expected = new CreatePostResponse(postId);

        var actual = await _sut.Handle(request, CancellationToken.None);

        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ThrowFeedNotFoundException_WhenFeedIsNotFound()
    {
        var postId = Guid.Parse("625DDFCE-A8C8-4343-B3D0-FAF6EDF74DC3");
        var request = new CreatePostRequest(
            "Post title",
            "Post description",
            ["Post category"],
            DateTime.UtcNow.AddHours(-1),
            $"example.com/posts/{postId}",
            Feed.Id);
        _storage.IsFeedExist(Arg.Any<Guid>(), CancellationToken.None).Returns(false);

        var actual = _sut.Invoking(s => s.Handle(request, CancellationToken.None));

        await actual.Should().ThrowExactlyAsync<FeedNotFoundException>();
        await _storage.Received(0).CreatePost(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Any<string[]>(),
            Arg.Any<DateTime>(),
            Arg.Any<string>(),
            Arg.Any<Guid>(),
            CancellationToken.None);
    }
}