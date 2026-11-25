using ECommerce.ApplicationCore.Interfaces;

namespace ECommerce.Infrastructure.Payments;

public sealed class UpiPaymentProcessor : IPaymentProcessor
{
    public string Name => "UPI";

    public Task<PaymentResult> ProcessAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PaymentResult(true, $"UPI-{Guid.NewGuid():N}"));
}
