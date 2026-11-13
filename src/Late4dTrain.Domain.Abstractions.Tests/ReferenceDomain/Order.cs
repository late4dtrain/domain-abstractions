namespace Late4dTrain.Domain.Abstractions.Tests.ReferenceDomain;

using System;
using System.Collections.Generic;

using Late4dTrain.Domain.Abstractions;

using ValueObjectBase = Late4dTrain.Domain.Abstractions.ValueObject;

internal readonly record struct OrderId(Guid Value);

internal readonly record struct CustomerId(Guid Value);

internal readonly record struct ProductId(Guid Value);

internal sealed record CurrencyCode
{
    public CurrencyCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 3)
        {
            throw new ArgumentException("Currency codes must contain three characters.", nameof(value));
        }

        Value = value.ToUpperInvariant();
    }

    public string Value { get; }
}

internal sealed class Money : ValueObjectBase
{
    public Money(decimal amount, CurrencyCode currency)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Money cannot be negative.");
        if (currency is null) throw new ArgumentNullException(nameof(currency));

        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public CurrencyCode Currency { get; }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}

internal sealed class OrderLine : Entity<Guid>
{
    public OrderLine(Guid id, ProductId productId, int quantity, Money unitPrice) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public ProductId ProductId { get; }

    public int Quantity { get; }

    public Money UnitPrice { get; }
}

internal sealed class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderLine> _lines = new();

    public Order(OrderId id, CustomerId customerId, CurrencyCode currency) : base(id)
    {
        if (currency is null) throw new ArgumentNullException(nameof(currency));

        CustomerId = customerId;
        Currency = currency;
    }

    public CustomerId CustomerId { get; }

    public CurrencyCode Currency { get; }

    public bool IsSubmitted { get; private set; }

    public IReadOnlyList<OrderLine> Lines => Array.AsReadOnly(_lines.ToArray());

    public void AddLine(ProductId productId, int quantity, Money unitPrice)
    {
        if (IsSubmitted) throw new InvalidOperationException("A submitted order cannot be changed.");
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be positive.");
        if (unitPrice is null) throw new ArgumentNullException(nameof(unitPrice));
        if (unitPrice.Currency != Currency) throw new InvalidOperationException("All order lines must use the order currency.");

        var line = new OrderLine(Guid.NewGuid(), productId, quantity, unitPrice);
        _lines.Add(line);
        RaiseDomainEvent(new OrderLineAdded(Id, line.Id, productId, quantity, unitPrice));
    }

    public void Submit()
    {
        if (IsSubmitted) throw new InvalidOperationException("The order has already been submitted.");
        if (_lines.Count == 0) throw new InvalidOperationException("An order must contain at least one line before submission.");

        IsSubmitted = true;
        RaiseDomainEvent(new OrderSubmitted(Id));
    }
}

internal sealed class OrderLineAdded : DomainEvent
{
    public OrderLineAdded(OrderId orderId, Guid lineId, ProductId productId, int quantity, Money unitPrice)
    {
        OrderId = orderId;
        LineId = lineId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public OrderId OrderId { get; }

    public Guid LineId { get; }

    public ProductId ProductId { get; }

    public int Quantity { get; }

    public Money UnitPrice { get; }
}

internal sealed class OrderSubmitted : DomainEvent
{
    public OrderSubmitted(OrderId orderId) => OrderId = orderId;

    public OrderId OrderId { get; }
}
