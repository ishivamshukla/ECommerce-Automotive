using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Notifications;

public sealed class SmsNotificationService : INotificationService
{
    public Task SendOrderConfirmationAsync(Order order, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"SMS confirmation sent to {order.Customer.PhoneNumber} for order {order.Id}.");
        return Task.CompletedTask;
    }
}
