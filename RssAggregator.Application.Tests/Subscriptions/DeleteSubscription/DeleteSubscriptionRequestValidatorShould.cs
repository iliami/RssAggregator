using FluentAssertions;
using RssAggregator.Application.UseCases.Subscriptions.DeleteSubscriptionUseCase;

namespace RssAggregator.Application.Tests.Subscriptions.DeleteSubscription;

public class DeleteSubscriptionRequestValidatorShould
{
    private readonly DeleteSubscriptionRequestValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenRequestIsValid()
    {
        var feedId = Guid.Parse("54F0ABEE-110E-4293-A4C0-0BF24F05888A");
        var request = new DeleteSubscriptionRequest(feedId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ReturnFailure_WhenRequestIsInvalid()
    {
        var feedId = Guid.Empty;
        var request = new DeleteSubscriptionRequest(feedId);

        var actual = _sut.Validate(request);

        actual.IsValid.Should().BeFalse();
    }
}