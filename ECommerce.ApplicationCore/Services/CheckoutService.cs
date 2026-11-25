using ECommerce.ApplicationCore.Interfaces;
using ECommerce.Domain.Entities;

namespace ECommerce.ApplicationCore.Services;

public sealed class CheckoutService(
    IOrderRepository orderRepository,
    IPaymentProcessor paymentProcessor,
    IEnumerable<INotificationService> notificationServices,
    IInvoiceService invoiceService)
{
    public async Task<CheckoutResult> CheckoutAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order '{orderId}' was not found.");

        var payment = await paymentProcessor.ProcessAsync(order.Id, order.TotalAmount, cancellationToken);
        if (!payment.IsSuccessful)
        {
            return new CheckoutResult(false, order.Id, null, payment.ErrorMessage);
        }

        order.MarkAsPaid();
        order.Confirm();
        var invoiceNumber = await invoiceService.GenerateAsync(order, cancellationToken);

        foreach (var notificationService in notificationServices)
        {
            await notificationService.SendOrderConfirmationAsync(order, cancellationToken);
        }

        return new CheckoutResult(true, order.Id, invoiceNumber, null);
    }
}

public sealed record CheckoutResult(bool IsSuccessful, Guid OrderId, string? InvoiceNumber, string? ErrorMessage);
