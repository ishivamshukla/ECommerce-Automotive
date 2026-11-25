namespace ECommerce.Domain.Entities;

public sealed class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; init; }
    public required string VehicleIdentificationNumber { get; init; }
    public decimal Price { get; init; }
    public bool IsAvailable { get; set; } = true;
}
