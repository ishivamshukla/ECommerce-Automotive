using ECommerce.ApplicationCore.Interfaces;

namespace ECommerce.Infrastructure.Payments;

public sealed class CardPaymentProcessor : IPaymentProcessor
{
    public string Name => "Card";

    public Task<PaymentResult> ProcessAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default) =>
        Task.FromResult(new PaymentResult(true, $"CARD-{Guid.NewGuid():N}"));
}
