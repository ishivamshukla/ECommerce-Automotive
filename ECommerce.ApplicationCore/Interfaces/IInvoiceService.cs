using ECommerce.Domain.Entities;

namespace ECommerce.ApplicationCore.Interfaces;

public interface IInvoiceService
{
    Task<string> GenerateAsync(Order order, CancellationToken cancellationToken = default);
}
