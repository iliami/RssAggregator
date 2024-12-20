using FluentAssertions;
using NSubstitute;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;

namespace RssAggregator.Application.Tests.Posts;

public class GetPostsUseCaseShould
{
    private readonly GetPostsUseCase _sut;
    private readonly IGetPostsStorage _storage;

    public GetPostsUseCaseShould()
    {
        _storage = Substitute.For<IGetPostsStorage>();
        _sut = new GetPostsUseCase(_storage);
    }

    [Fact]
    public async Task ReturnResponseWithoutPosts_WhenNoPosts()
    {
        var request = new GetPostsRequest();
        _storage
            .GetPosts(
                Arg.Any<PaginationParams>(),
                Arg.Any<SortingParams>(),
                Arg.Any<PostFilterParams>(),
                Arg.Any<CancellationToken>())
            .Returns(new PagedResult<PostDto>([], 0));
        var expected = new GetPostsResponse(new PagedResult<PostDto>(Array.Empty<PostDto>(), 0));

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).GetPosts(
            Arg.Any<PaginationParams>(),
            Arg.Any<SortingParams>(),
            Arg.Any<PostFilterParams>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnResponseWithPosts_WhenRequestIsValid()
    {
        var posts = GeneratePosts(20);
        var request = new GetPostsRequest(
            new PaginationParams { Page = 1, PageSize = posts.Length },
            new SortingParams { SortBy = "PublishDate", SortDirection = SortDirection.Desc },
            new PostFilterParams { Categories = [] });
        _storage
            .GetPosts(
                Arg.Any<PaginationParams>(),
                Arg.Any<SortingParams>(),
                Arg.Any<PostFilterParams>(),
                Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var pagination = callInfo.Arg<PaginationParams>();
                var page = pagination.Page;
                var pageSize = pagination.PageSize;

                var paginatedPosts = posts
                    .Skip(( page - 1 ) * pageSize)
                    .Take(pageSize)
                    .ToArray();

                return new PagedResult<PostDto>(paginatedPosts, posts.Length);
            });
        var expected = new GetPostsResponse(new PagedResult<PostDto>(posts, posts.Length));

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage.Received(1).GetPosts(
            Arg.Any<PaginationParams>(),
            Arg.Any<SortingParams>(),
            Arg.Any<PostFilterParams>(),
            Arg.Any<CancellationToken>());
    }

    private static PostDto[] GeneratePosts(int count)
    {
        return Enumerable.Range(0, count)
            .Select(i => new PostDto(
                Guid.NewGuid(),
                $"Post {i}",
                Array.Empty<string>(),
                DateTime.UtcNow,
                $"Content {i}",
                Guid.NewGuid()))
            .ToArray();
    }
}
