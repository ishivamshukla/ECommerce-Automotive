using ECommerce.Domain.Entities;

namespace ECommerce.ApplicationCore.Interfaces;

public interface INotificationService
{
    Task SendOrderConfirmationAsync(Order order, CancellationToken cancellationToken = default);
}
