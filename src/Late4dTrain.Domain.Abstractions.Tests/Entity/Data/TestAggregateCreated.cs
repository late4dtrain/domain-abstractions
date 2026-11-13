namespace Late4dTrain.Domain.Abstractions.Tests.Entity.Data;

public class TestAggregateCreated : DomainEvent
{
    public string EventName => nameof(TestAggregateCreated);
}
