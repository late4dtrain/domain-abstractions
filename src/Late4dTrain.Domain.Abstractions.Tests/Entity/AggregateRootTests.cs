namespace Late4dTrain.Domain.Abstractions.Tests.Entity;

using System;
using System.Collections.Generic;

using FluentAssertions;

using Xunit;

public class AggregateRootTests
{
    [Fact]
    public void New_Aggregate_Has_No_Pending_Events()
    {
        var aggregate = new TestRoot(Guid.NewGuid());

        aggregate.CaptureDomainEvents().Should().BeEmpty();
    }

    [Fact]
    public void Aggregate_Root_Identity_Contract_Is_Independent_Of_Event_Contract()
    {
        var aggregate = new TestRoot(Guid.NewGuid());

        aggregate.Should().BeAssignableTo<IAggregateRoot>();
        aggregate.Should().BeAssignableTo<IAggregateRoot<Guid>>();
        aggregate.Should().BeAssignableTo<IDomainEventContainer>();
    }

    [Fact]
    public void Events_Are_Captured_In_Insertion_Order_Across_Event_Types()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        var first = new FirstEvent();
        var second = new SecondEvent();
        aggregate.Raise(first);
        aggregate.Raise(second);

        aggregate.CaptureDomainEvents().Should().Equal(first, second);
    }

    [Fact]
    public void Capture_Returns_A_Stable_ReadOnly_Snapshot_Not_The_Mutable_Buffer()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        var first = new FirstEvent();
        aggregate.Raise(first);
        var snapshot = aggregate.CaptureDomainEvents();
        aggregate.Raise(new SecondEvent());

        snapshot.Should().ContainSingle().Which.Should().BeSameAs(first);
        snapshot.Should().NotBeOfType<List<IDomainEvent>>();
        ((ICollection<IDomainEvent>)snapshot).IsReadOnly.Should().BeTrue();
        FluentActions.Invoking(() => ((ICollection<IDomainEvent>)snapshot).Add(new FirstEvent()))
            .Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Capturing_Without_Acknowledging_Retains_Events_For_Retry()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        var raised = new FirstEvent();
        aggregate.Raise(raised);

        var failedAttempt = aggregate.CaptureDomainEvents();
        failedAttempt.Should().ContainSingle().Which.Should().BeSameAs(raised);
        aggregate.CaptureDomainEvents().Should().ContainSingle().Which.Should().BeSameAs(raised);
    }

    [Fact]
    public void Acknowledging_A_Captured_Batch_Preserves_Events_Raised_After_Capture()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        var capturedEvent = new FirstEvent();
        aggregate.Raise(capturedEvent);
        var captured = aggregate.CaptureDomainEvents();
        var laterEvent = new SecondEvent();
        aggregate.Raise(laterEvent);

        aggregate.AcknowledgeDomainEvents(captured);

        aggregate.CaptureDomainEvents().Should().ContainSingle().Which.Should().BeSameAs(laterEvent);
    }

    [Fact]
    public void Acknowledging_The_Same_Batch_Twice_Is_Idempotent()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        aggregate.Raise(new FirstEvent());
        var captured = aggregate.CaptureDomainEvents();

        aggregate.AcknowledgeDomainEvents(captured);
        aggregate.AcknowledgeDomainEvents(captured);

        aggregate.CaptureDomainEvents().Should().BeEmpty();
    }

    [Fact]
    public void Acknowledgement_Removes_Only_Captured_Occurrences_When_An_Event_Instance_Is_Duplicated()
    {
        var aggregate = new TestRoot(Guid.NewGuid());
        var sameInstance = new FirstEvent();
        aggregate.Raise(sameInstance);
        aggregate.Raise(sameInstance);
        var captured = aggregate.CaptureDomainEvents();
        aggregate.Raise(sameInstance);

        aggregate.AcknowledgeDomainEvents(captured);

        aggregate.CaptureDomainEvents().Should().ContainSingle().Which.Should().BeSameAs(sameInstance);
    }

    [Fact]
    public void Raising_Null_Event_Is_Rejected()
    {
        var aggregate = new TestRoot(Guid.NewGuid());

        FluentActions.Invoking(() => aggregate.Raise(null!))
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("domainEvent");
    }

    [Fact]
    public void Acknowledging_Null_Collection_Is_Rejected()
    {
        var aggregate = new TestRoot(Guid.NewGuid());

        FluentActions.Invoking(() => aggregate.AcknowledgeDomainEvents(null!))
            .Should().Throw<ArgumentNullException>()
            .WithParameterName("capturedEvents");
    }

    private sealed class TestRoot : AggregateRoot<Guid>
    {
        public TestRoot(Guid id) : base(id)
        {
        }

        public void Raise(IDomainEvent domainEvent) => RaiseDomainEvent(domainEvent);
    }

    private sealed class FirstEvent : DomainEvent
    {
    }

    private sealed class SecondEvent : DomainEvent
    {
    }
}
