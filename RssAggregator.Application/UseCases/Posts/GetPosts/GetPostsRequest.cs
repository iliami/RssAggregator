using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public record GetPostsRequest(Specification<Post> Specification);