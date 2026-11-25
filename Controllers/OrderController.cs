using ECommerce.ApplicationCore.Services;
using ECommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Application.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrderController(OrderService orderService, CheckoutService checkoutService) : ControllerBase
{
    [HttpPost("book")]
    public async Task<ActionResult<BookVehicleResponse>> BookVehicle(BookVehicleRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer { Name = request.CustomerName, Email = request.Email, PhoneNumber = request.PhoneNumber };
        var vehicle = new Product { Name = request.VehicleName, VehicleIdentificationNumber = request.VehicleIdentificationNumber, Price = request.Price };
        var order = await orderService.BookVehicleAsync(customer, vehicle, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { orderId = order.Id }, new BookVehicleResponse(order.Id, order.Status.ToString(), order.TotalAmount));
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<Order>> GetOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await orderService.GetOrderAsync(orderId, cancellationToken);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost("{orderId:guid}/checkout")]
    public async Task<ActionResult<CheckoutResult>> Checkout(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await checkoutService.CheckoutAsync(orderId, cancellationToken);
            return result.IsSuccessful ? Ok(result) : BadRequest(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}

public sealed record BookVehicleRequest(string CustomerName, string Email, string PhoneNumber, string VehicleName, string VehicleIdentificationNumber, decimal Price);
public sealed record BookVehicleResponse(Guid OrderId, string Status, decimal TotalAmount);