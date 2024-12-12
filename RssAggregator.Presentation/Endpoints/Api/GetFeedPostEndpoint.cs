// using FastEndpoints;
// using RssAggregator.Application.Abstractions.Repositories;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record GetFeedPostResponse(
//     Guid Id,
//     string Title,
//     string Description,
//     string Category,
//     DateTime PublishDate,
//     string Url,
//     Guid FeedId);
//
// public class GetFeedPostEndpoint(IPostRepository PostRepository) : EndpointWithoutRequest<GetFeedPostResponse>
// {
//     public override void Configure()
//     {
//         Get("api/posts/{id:guid}");
//     }
//
//     public override async Task<GetFeedPostResponse> ExecuteAsync(CancellationToken ct)
//     {
//         var postId = Route<Guid>("id");
//
//         var post = await PostRepository.GetByIdAsync(postId, ct);
//
//         if (post is null) ThrowError("Post not found", StatusCodes.Status404NotFound);
//
//         var res = new GetFeedPostResponse(
//             post.Id,
//             post.Title,
//             post.Description,
//             post.Category,
//             post.PublishDate,
//             post.Url,
//             post.Feed.Id);
//
//         return res;
//     }
// }