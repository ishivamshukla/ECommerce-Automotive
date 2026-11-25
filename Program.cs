var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ECommerce.ApplicationCore.Interfaces.IOrderRepository, ECommerce.Infrastructure.Repositories.InMemoryOrderRepository>();
builder.Services.AddSingleton<ECommerce.ApplicationCore.Interfaces.IPaymentProcessor, ECommerce.Infrastructure.Payments.UpiPaymentProcessor>();
builder.Services.AddSingleton<ECommerce.ApplicationCore.Interfaces.IInvoiceService, ECommerce.Infrastructure.Invoices.InvoiceService>();
builder.Services.AddSingleton<ECommerce.ApplicationCore.Interfaces.INotificationService, ECommerce.Infrastructure.Notifications.EmailNotificationService>();
builder.Services.AddSingleton<ECommerce.ApplicationCore.Interfaces.INotificationService, ECommerce.Infrastructure.Notifications.SmsNotificationService>();
builder.Services.AddScoped<ECommerce.ApplicationCore.Services.OrderService>();
builder.Services.AddScoped<ECommerce.ApplicationCore.Services.CheckoutService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
