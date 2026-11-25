using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.ApplicationCore.Services;

public sealed class OrderService(IOrderRepository orderRepository)
{
    public async Task<Order> BookVehicleAsync(Customer customer, Product vehicle, CancellationToken cancellationToken = default)
    {
        if (!vehicle.IsAvailable)
        {
            throw new InvalidOperationException("The vehicle is not available for booking.");
        }

        vehicle.IsAvailable = false;
        var order = new Order { Customer = customer, Vehicle = vehicle };
        await orderRepository.AddAsync(order, cancellationToken);
        return order;
    }

    public Task<Order?> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default) =>
        orderRepository.GetByIdAsync(orderId, cancellationToken);
}
