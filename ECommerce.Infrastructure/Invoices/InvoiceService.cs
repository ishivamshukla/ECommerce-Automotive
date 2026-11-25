using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Invoices;

public sealed class InvoiceService : IInvoiceService
{
    public Task<string> GenerateAsync(Order order, CancellationToken cancellationToken = default) =>
        Task.FromResult($"INV-{DateTime.UtcNow:yyyyMMdd}-{order.Id.ToString()[..8].ToUpperInvariant()}");
}
