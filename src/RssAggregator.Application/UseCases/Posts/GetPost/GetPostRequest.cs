using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public record GetPostRequest(Guid PostId, Specification<Post> Specification);