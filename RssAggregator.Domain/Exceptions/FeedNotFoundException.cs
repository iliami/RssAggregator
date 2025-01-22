using RssAggregator.Domain.Entities;

namespace RssAggregator.Domain.Exceptions;

public class FeedNotFoundException(Guid feedId) 
    : Exception($"The {nameof(Feed)} with the id \"{feedId}\" could not be found."), 
        INotFoundException;