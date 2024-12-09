using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.DTO;

public record SubscriptionDto(AppUser User, Feed Feed);