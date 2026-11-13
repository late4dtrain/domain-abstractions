namespace Late4dTrain.Domain.Abstractions.Tests.ReferenceDomain;

using System;

using AutoFixture.Xunit2;

using FluentAssertions;

using TestStack.BDDfy;

using Xunit;

public class OrderTests
{
    [Theory]
    [AutoData]
    public void AddLine_Records_An_Event_And_Owns_The_Line(Guid orderGuid, Guid productGuid)
    {
        var order = CreateOrder(orderGuid);
        var price = new Money(12.50m, new CurrencyCode("usd"));
        var productId = new ProductId(productGuid);

        order.AddLine(productId, 2, price);

        order.Lines.Should().ContainSingle().Which.ProductId.Should().Be(productId);
        order.Lines[0].Quantity.Should().Be(2);
        order.CaptureDomainEvents().Should().ContainSingle().Which.Should().BeOfType<OrderLineAdded>();
    }

    [Fact]
    public void Submit_Requires_Lines_And_Failed_Operation_Leaves_State_And_Events_Unchanged()
    {
        var order = CreateOrder();
        var before = order.CaptureDomainEvents();

        FluentActions.Invoking(order.Submit).Should().Throw<InvalidOperationException>();

        order.IsSubmitted.Should().BeFalse();
        order.Lines.Should().BeEmpty();
        order.CaptureDomainEvents().Should().Equal(before);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NonPositive_Quantity_Is_Rejected_Without_Mutation(int quantity)
    {
        var order = CreateOrder();

        FluentActions.Invoking(() => order.AddLine(new ProductId(Guid.NewGuid()), quantity, Usd(1m)))
            .Should().Throw<ArgumentOutOfRangeException>();

        order.Lines.Should().BeEmpty();
        order.CaptureDomainEvents().Should().BeEmpty();
    }

    [Fact]
    public void Incompatible_Currency_Is_Rejected_Without_Mutation()
    {
        var order = CreateOrder();

        FluentActions.Invoking(() => order.AddLine(new ProductId(Guid.NewGuid()), 1, new Money(1m, new CurrencyCode("eur"))))
            .Should().Throw<InvalidOperationException>();

        order.Lines.Should().BeEmpty();
        order.CaptureDomainEvents().Should().BeEmpty();
    }

    [Fact]
    public void Negative_Money_Is_Invalid()
    {
        FluentActions.Invoking(() => new Money(-0.01m, new CurrencyCode("USD")))
            .Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Invalid_Or_Null_Currency_Cannot_Create_Valid_Domain_Values()
    {
        FluentActions.Invoking(() => new CurrencyCode("US"))
            .Should().Throw<ArgumentException>();
        FluentActions.Invoking(() => new Money(1m, null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("currency");
        FluentActions.Invoking(() => new Order(new OrderId(Guid.NewGuid()), new CustomerId(Guid.NewGuid()), null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("currency");
    }

    [Fact]
    public void Submitted_Order_Cannot_Be_Changed_And_Failure_Does_Not_Add_Events()
    {
        var order = CreateOrder();
        order.AddLine(new ProductId(Guid.NewGuid()), 1, Usd(1m));
        order.Submit();
        var eventsBefore = order.CaptureDomainEvents();

        FluentActions.Invoking(() => order.AddLine(new ProductId(Guid.NewGuid()), 1, Usd(1m)))
            .Should().Throw<InvalidOperationException>();

        order.IsSubmitted.Should().BeTrue();
        order.Lines.Should().ContainSingle();
        order.CaptureDomainEvents().Should().Equal(eventsBefore);
    }

    [Fact]
    public void Caller_Cannot_Mutate_The_Aggregate_Line_Collection()
    {
        var order = CreateOrder();
        order.AddLine(new ProductId(Guid.NewGuid()), 1, Usd(1m));
        var lines = order.Lines;

        ((System.Collections.Generic.ICollection<OrderLine>)lines).IsReadOnly.Should().BeTrue();
        FluentActions.Invoking(() => ((System.Collections.Generic.ICollection<OrderLine>)lines).Clear())
            .Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Successful_Transitions_Record_Events_In_Order()
    {
        var order = CreateOrder();
        order.AddLine(new ProductId(Guid.NewGuid()), 1, Usd(5m));

        this.Given(_ => order.Submit())
            .Then(_ => order.IsSubmitted.Should().BeTrue())
            .BDDfy();

        order.CaptureDomainEvents().Should().HaveCount(2);
        order.CaptureDomainEvents()[0].Should().BeOfType<OrderLineAdded>();
        order.CaptureDomainEvents()[1].Should().BeOfType<OrderSubmitted>();
    }

    private static Order CreateOrder(Guid? id = null) =>
        new(new OrderId(id ?? Guid.NewGuid()), new CustomerId(Guid.NewGuid()), new CurrencyCode("USD"));

    private static Money Usd(decimal amount) => new(amount, new CurrencyCode("USD"));
}
