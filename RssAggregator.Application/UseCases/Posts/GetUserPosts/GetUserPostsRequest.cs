using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public record GetUserPostsRequest(Specification<Post> Specification);