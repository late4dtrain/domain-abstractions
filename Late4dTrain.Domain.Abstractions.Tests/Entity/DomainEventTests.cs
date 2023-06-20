namespace Late4dTrain.Domain.Abstractions.Tests.Entity;

using System;

using Data;

using FluentAssertions;

using Xunit;

public class DomainEventTests
{
    [Fact]
    public void After_TestAggregateRoot_Is_Created_DomainEvent_Is_Added()
    {
        var testAggregateRoot = new TestAggregateRoot(Guid.NewGuid(), "test");
        testAggregateRoot.Events.Should().HaveCount(1);
        testAggregateRoot.Events[0].Should().BeOfType<TestAggregateCreated>();
    }
}
