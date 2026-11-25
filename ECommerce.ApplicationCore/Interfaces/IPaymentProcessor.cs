namespace ECommerce.ApplicationCore.Interfaces;

public interface IPaymentProcessor
{
    string Name { get; }
    Task<PaymentResult> ProcessAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default);
}

public sealed record PaymentResult(bool IsSuccessful, string TransactionId, string? ErrorMessage = null);
