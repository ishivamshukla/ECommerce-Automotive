using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Notifications;

public sealed class EmailNotificationService : INotificationService
{
    public Task SendOrderConfirmationAsync(Order order, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Email confirmation sent to {order.Customer.Email} for order {order.Id}.");
        return Task.CompletedTask;
    }
}
