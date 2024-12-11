using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Models.DTO;

public record SubscriptionDto(AppUser User, Feed Feed);