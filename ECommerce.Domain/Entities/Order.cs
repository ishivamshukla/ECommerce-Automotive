using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Customer Customer { get; init; }
    public required Product Vehicle { get; init; }
    public DateTime BookingDateUtc { get; init; } = DateTime.UtcNow;
    public decimal TotalAmount => Vehicle.Price;
    public OrderStatus Status { get; private set; } = OrderStatus.PendingPayment;

    public void MarkAsPaid() => Status = OrderStatus.Paid;
    public void Confirm() => Status = OrderStatus.Confirmed;
    public void Cancel() => Status = OrderStatus.Cancelled;
}
