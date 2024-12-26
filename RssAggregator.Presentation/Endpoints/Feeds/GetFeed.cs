using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetFeed : IEndpoint
{
    private record GetFeedPostModel(Guid Id, string Title, string Url, DateTime PublishDate, IEnumerable<string> Categories);

    private record GetFeedModel(
        Guid Id,
        string Name,
        string Description,
        int Subscribers,
        IEnumerable<GetFeedPostModel> Posts);

    private class GetFeedSpecification : Specification<Feed, GetFeedModel>
    {
        public GetFeedSpecification(Guid feedId) : base(feed => new GetFeedModel(
            feed.Id, 
            feed.Name, 
            feed.Description, 
            feed.Subscribers.Count,
            feed.Posts.Select(post => new GetFeedPostModel(
                post.Id, 
                post.Title, 
                post.Url, 
                post.PublishDate,
                post.Categories.Select(category => category.Name)))))
        {
            IsNoTracking = true;
            
            Criteria = feed => feed.Id == feedId;
        }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] IGetFeedUseCase<GetFeedModel> useCase,
            CancellationToken ct) =>
        {
            var request = new GetFeedRequest<GetFeedModel>(id, new GetFeedSpecification(id));
            var response = await useCase.Handle(request, ct);

            return Results.Ok(response);
        }).RequireAuthorization().WithTags(EndpointsTags.Feeds);
    }
}