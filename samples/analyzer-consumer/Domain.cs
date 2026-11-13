namespace AnalyzerConsumer;

using System;
using System.Collections.Generic;

using Late4dTrain.Domain.Abstractions;

public sealed class Customer : Entity<Guid>
{
    public Customer(Guid id) : base(id)
    {
    }
}

public sealed class Order : AggregateRoot<Guid>
{
    public Order(Guid id) : base(id)
    {
    }

    public IReadOnlyList<Guid> ProductIds { get; } = Array.Empty<Guid>();
}

public sealed class Money : ValueObject
{
    public Money(decimal amount, string currency) => (Amount, Currency) = (amount, currency);

    public decimal Amount { get; }

    public string Currency { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}
