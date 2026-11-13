namespace Late4dTrain.Domain.Abstractions.Tests.ReferenceDomain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using FluentAssertions;

using Late4dTrain.Domain.Abstractions;

using Xunit;

public class OrderRepositoryTests
{
    [Fact]
    public async Task Repository_Uses_Root_Identity_And_Reports_Missing_Aggregates_As_Null()
    {
        IOrderRepository repository = new InMemoryOrderRepository();
        var id = new OrderId(Guid.NewGuid());
        var order = CreateOrder(id);

        await repository.AddAsync(order);

        (await repository.GetByIdAsync(id)).Should().BeSameAs(order);
        (await repository.GetByIdAsync(new OrderId(Guid.NewGuid()))).Should().BeNull();
    }

    [Fact]
    public async Task Update_Persists_An_Existing_Aggregate_But_Rejects_Unknown_Identity()
    {
        var repository = new InMemoryOrderRepository();
        var order = CreateOrder(new OrderId(Guid.NewGuid()));
        await repository.AddAsync(order);
        order.AddLine(new ProductId(Guid.NewGuid()), 1, new Money(2m, new CurrencyCode("USD")));

        await repository.UpdateAsync(order);

        (await repository.GetByIdAsync(order.Id))!.Lines.Should().ContainSingle();
        await FluentActions.Awaiting(() => repository.UpdateAsync(CreateOrder(new OrderId(Guid.NewGuid()))))
            .Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task Cancellation_Is_Forwarded_And_Honored()
    {
        var repository = new InMemoryOrderRepository();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await FluentActions.Awaiting(() => repository.GetByIdAsync(new OrderId(Guid.NewGuid()), cancellation.Token))
            .Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void Domain_Specific_Read_Projection_Does_Not_Expose_IQueryable()
    {
        typeof(IOrderQueries).GetMethods().SelectMany(method => method.GetParameters())
            .Should().NotContain(parameter => parameter.ParameterType.IsGenericType &&
                parameter.ParameterType.GetGenericTypeDefinition().FullName == "System.Linq.IQueryable`1");
    }

    [Fact]
    public void Repository_Constraint_Requires_An_Aggregate_Root_Not_A_Child_Or_ValueObject()
    {
        typeof(Order).Should().BeAssignableTo<IAggregateRoot<OrderId>>();
        typeof(OrderLine).Should().NotBeAssignableTo<IAggregateRoot<Guid>>();
        typeof(Money).Should().NotBeAssignableTo<IAggregateRoot<OrderId>>();

        var aggregateConstraint = typeof(IAggregateRepository<,>).GetGenericArguments()[0]
            .GetGenericParameterConstraints()
            .Single(constraint => constraint.IsGenericType &&
                constraint.GetGenericTypeDefinition() == typeof(IAggregateRoot<>));
        aggregateConstraint.GetGenericTypeDefinition().Should().Be(typeof(IAggregateRoot<>));
    }

    private static Order CreateOrder(OrderId id) =>
        new(id, new CustomerId(Guid.NewGuid()), new CurrencyCode("USD"));

    private interface IOrderRepository : IAggregateRepository<Order, OrderId>
    {
    }

    private interface IOrderQueries
    {
        Task<OrderSummary?> GetSummaryAsync(OrderId id, CancellationToken cancellationToken = default);
    }

    private sealed record OrderSummary(OrderId Id, bool IsSubmitted, int LineCount);

    private sealed class InMemoryOrderRepository : IOrderRepository
    {
        private readonly Dictionary<OrderId, Order> _orders = new();

        public Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _orders.TryGetValue(id, out var order);
            return Task.FromResult(order);
        }

        public Task AddAsync(Order aggregate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_orders.TryAdd(aggregate.Id, aggregate))
            {
                throw new InvalidOperationException("An aggregate with this identity already exists.");
            }

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Order aggregate, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_orders.ContainsKey(aggregate.Id))
            {
                throw new InvalidOperationException("The aggregate does not exist.");
            }

            _orders[aggregate.Id] = aggregate;
            return Task.CompletedTask;
        }
    }
}
