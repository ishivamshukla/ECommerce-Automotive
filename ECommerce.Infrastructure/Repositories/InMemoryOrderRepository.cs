using System.Collections.Concurrent;
using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Repositories;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> orders = new();

    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        orders[order.Id] = order;
        return Task.CompletedTask;
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }
}
