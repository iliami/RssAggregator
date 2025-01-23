using RssAggregator.Domain.Entities;

namespace RssAggregator.Domain.Exceptions;

public class UserNotFoundException(Guid userId) 
    : Exception($"The {nameof(User)} with the id \"{userId}\" could not be found."), 
        INotFoundException;