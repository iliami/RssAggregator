using FluentAssertions;
using NSubstitute;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;

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
    public async Task ReturnEmpty_WhenNoPosts()
    {
        var request = new GetPostsRequest();
        _storage
            .GetPosts(
                Arg.Any<PaginationParams>(),
                Arg.Any<SortingParams>(),
                Arg.Any<PostFilterParams>(),
                Arg.Any<CancellationToken>())
            .Returns(new PagedResult<PostDto>(Array.Empty<PostDto>(), 0));
        var expected = new GetPostsResponse(new PagedResult<PostDto>(Array.Empty<PostDto>(), 0));

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage
            .Received(1)
            .GetPosts(
                Arg.Any<PaginationParams>(),
                Arg.Any<SortingParams>(),
                Arg.Any<PostFilterParams>(),
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ReturnPosts_WhenRequestIsValid()
    {
        var posts = Enumerable.Range(0, 20)
            .Select(_ => new PostDto(
                Guid.Parse("2B36C46E-B804-4C2C-A5C7-3831B5377202"),
                "",
                [],
                DateTime.UtcNow,
                "",
                Guid.Empty))
            .ToArray();
        var paginationParams = new PaginationParams
        {
            Page = 1,
            PageSize = posts.Length
        };
        var sortingParams = new SortingParams
        {
            SortBy = string.Empty,
            SortDirection = SortDirection.None
        };
        var postFilterParams = new PostFilterParams
        {
            Categories = []
        };
        
        var request = new GetPostsRequest(
            paginationParams,
            sortingParams,
            postFilterParams);
        _storage
            .GetPosts(
                Arg.Is<PaginationParams>(pp => pp.Page * pp.PageSize >= posts.Length),
                Arg.Any<SortingParams>(),
                Arg.Is<PostFilterParams>(pfp => pfp.Categories.Length == 0),
                Arg.Any<CancellationToken>())
            .Returns(new PagedResult<PostDto>(posts, posts.Length));
        var expected = new GetPostsResponse(new PagedResult<PostDto>(posts, posts.Length));

        var actual = await _sut.Handle(request);

        actual.Should().BeEquivalentTo(expected);
        await _storage
            .Received(1)
            .GetPosts(
                Arg.Any<PaginationParams>(),
                Arg.Any<SortingParams>(),
                Arg.Any<PostFilterParams>(),
                Arg.Any<CancellationToken>());
    }
}