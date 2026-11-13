namespace Late4dTrain.Domain.Abstractions.Tests.Entity;

using System;

using Data;

using FluentAssertions;

using TestStack.BDDfy;

using Xunit;

public class DomainEventTests
{
    private TestAggregateRoot? _aggregateRoot;

    [Fact]
    public void After_TestAggregateRoot_Is_Created_DomainEvent_Is_Added()
    {
        this.When(_ => CreateAggregate())
            .Then(_ => VerifyCreatedEvent())
            .BDDfy();
    }

    [Fact]
    public void Explicit_Occurrence_Time_Is_Normalized_To_UTC_And_Stored()
    {
        var occurrence = new DateTimeOffset(2024, 3, 4, 10, 15, 0, TimeSpan.FromHours(5));
        var domainEvent = new TimestampedEvent(occurrence);

        domainEvent.DateOccurred.Should().Be(occurrence.ToUniversalTime());
        domainEvent.DateOccurred.Offset.Should().Be(TimeSpan.Zero);
    }

    [Fact]
    public void Convenience_Occurrence_Time_Is_Captured_Within_Creation_Bounds()
    {
        var before = DateTimeOffset.UtcNow;
        var domainEvent = new TestAggregateCreated();
        var after = DateTimeOffset.UtcNow;

        domainEvent.DateOccurred.Should().BeOnOrAfter(before);
        domainEvent.DateOccurred.Should().BeOnOrBefore(after);
        domainEvent.DateOccurred.Offset.Should().Be(TimeSpan.Zero);
    }

    private void CreateAggregate() => _aggregateRoot = new TestAggregateRoot(Guid.NewGuid(), "test");

    private void VerifyCreatedEvent() => _aggregateRoot!.CaptureDomainEvents().Should().ContainSingle()
        .Which.Should().BeOfType<TestAggregateCreated>();

    private sealed class TimestampedEvent : DomainEvent
    {
        public TimestampedEvent(DateTimeOffset dateOccurred) : base(dateOccurred)
        {
        }
    }
}
