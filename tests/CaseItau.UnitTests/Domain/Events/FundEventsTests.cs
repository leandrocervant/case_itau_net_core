using CaseItau.Domain.Events.Fund;
using CaseItau.UnitTests.Common.Builders;
using FluentAssertions;
using Xunit;

namespace CaseItau.UnitTests.Domain.Events;

public class FundEventsTests
{
    [Fact]
    public void Fund_WhenCreated_ShouldRaiseFundCreatedEvent()
    {
        // Arrange & Act
        var fund = FundBuilder.New()
            .WithCode("EVENT_001")
            .WithName("Event Test Fund")
            .Build();

        // Assert
        var events = fund.PopEvents();
        events.Should().HaveCount(1);

        var fundCreatedEvent = events.First().Should().BeOfType<FundCreatedEvent>().Subject;
        fundCreatedEvent.Id.Should().Be(fund.Id);
        fundCreatedEvent.Code.Should().Be("EVENT_001");
        fundCreatedEvent.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Fund_PopEvents_ShouldClearEventsAfterRetrieving()
    {
        // Arrange
        var fund = FundBuilder.New().Build();

        // Act
        var firstCall = fund.PopEvents();
        var secondCall = fund.PopEvents();

        // Assert
        firstCall.Should().HaveCount(1);
        secondCall.Should().BeEmpty();
    }

    [Fact]
    public void FundCreatedEvent_ShouldHaveCorrectProperties()
    {
        // Arrange
        var fundId = 1;
        const string code = "EVENT_002";
        var createdAt = DateTime.UtcNow;

        // Act
        var fundCreatedEvent = new FundCreatedEvent(fundId, code, createdAt);

        // Assert
        fundCreatedEvent.Id.Should().Be(fundId);
        fundCreatedEvent.Code.Should().Be(code);
        fundCreatedEvent.CreatedAt.Should().Be(createdAt);
    }
}
