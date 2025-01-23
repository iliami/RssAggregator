using RssAggregator.Domain.Entities;

namespace RssAggregator.Domain.Exceptions;

public class PostNotFoundException(Guid postId) 
    : Exception($"The {nameof(Post)} with the id \"{postId}\" could not be found."), 
        INotFoundException;