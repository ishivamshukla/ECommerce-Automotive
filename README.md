# ECommerce Automotive

A practical .NET 10 automotive e-commerce API demonstrating all five SOLID principles through vehicle booking, order management, payment processing, notifications, and invoice generation.

## What the project does

The API supports this workflow:

1. A customer books an available vehicle.
2. The system creates an order with `PendingPayment` status.
3. Checkout sends the order to a payment processor.
4. A successful payment changes the order to `Paid`, then `Confirmed`.
5. An invoice is generated.
6. Email and SMS confirmation services are called.

The current implementation uses in-memory storage and simulated payment, invoice, email, and SMS integrations. It is intended as an architecture example and starting point, not a production payment system.

## Technology

- .NET 10
- ASP.NET Core Web API
- C# with nullable reference types and implicit usings enabled
- OpenAPI in development
- In-memory order persistence
- GitHub repository: https://github.com/ishivamshukla/ECommerce-Automotive

## Project structure

```text
ECommerce.Application/
├── Controllers/
│   └── OrderController.cs
├── ECommerce.Application.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── ECommerce.Application.http
├── Properties/
│   └── launchSettings.json
├── ECommerce.Domain/
│   ├── Entities/
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   └── Product.cs
│   ├── Enums/
│   │   └── OrderStatus.cs
│   └── ECommerce.Domain.csproj
├── ECommerce.ApplicationCore/
│   ├── Interfaces/
│   │   ├── IInvoiceService.cs
│   │   ├── INotificationService.cs
│   │   ├── IOrderRepository.cs
│   │   └── IPaymentProcessor.cs
│   ├── Services/
│   │   ├── CheckoutService.cs
│   │   └── OrderService.cs
│   └── ECommerce.ApplicationCore.csproj
└── ECommerce.Infrastructure/
	├── Invoices/
	│   └── InvoiceService.cs
	├── Notifications/
	│   ├── EmailNotificationService.cs
	│   └── SmsNotificationService.cs
	├── Payments/
	│   ├── CardPaymentProcessor.cs
	│   └── UpiPaymentProcessor.cs
	├── Repositories/
	│   └── InMemoryOrderRepository.cs
	└── ECommerce.Infrastructure.csproj
```

## Architecture and dependency direction

The solution is split into four logical layers:

```text
API host (ECommerce.Application)
		↓
Infrastructure implementations
		↓
ApplicationCore interfaces and services
		↓
Domain entities and rules
```

The API host is the composition root. It connects interfaces to concrete infrastructure implementations in `Program.cs`. Application services do not instantiate repositories, payment gateways, or notification providers directly.

### ECommerce.Domain

Contains business data and rules without references to ASP.NET Core, databases, or external services.

- `Customer.cs` represents the customer placing an order.
- `Product.cs` represents a vehicle, including its VIN, price, and availability.
- `Order.cs` connects a customer and vehicle and owns order state transitions.
- `OrderStatus.cs` defines `PendingPayment`, `Paid`, `Confirmed`, and `Cancelled`.
- `ECommerce.Domain.csproj` defines the .NET 10 domain class library.

### ECommerce.ApplicationCore

Contains use cases and contracts owned by the application.

#### Interfaces

- `IOrderRepository.cs` abstracts order persistence.
- `IPaymentProcessor.cs` abstracts payment processing and returns a `PaymentResult`.
- `INotificationService.cs` abstracts order confirmation notifications. Multiple implementations can be registered.
- `IInvoiceService.cs` abstracts invoice generation.

#### Services

- `OrderService.cs` validates vehicle availability, marks a vehicle as booked, creates an order, and saves it.
- `CheckoutService.cs` loads the order, processes payment, confirms the order, generates an invoice, and sends all registered notifications.
- `ECommerce.ApplicationCore.csproj` references only the Domain project.

### ECommerce.Infrastructure

Contains replaceable technical implementations of ApplicationCore contracts.

- `InMemoryOrderRepository.cs` stores orders in a thread-safe in-memory dictionary. Replace this with an EF Core or other database repository for persistence.
- `UpiPaymentProcessor.cs` simulates a successful UPI payment.
- `CardPaymentProcessor.cs` simulates a successful card payment.
- `EmailNotificationService.cs` simulates sending an email confirmation.
- `SmsNotificationService.cs` simulates sending an SMS confirmation.
- `InvoiceService.cs` generates an invoice number.
- `ECommerce.Infrastructure.csproj` references Domain and ApplicationCore, but ApplicationCore does not reference Infrastructure.

### ECommerce.Application

This is the ASP.NET Core API host. The project name is retained for compatibility with the existing workspace.

- `OrderController.cs` exposes booking, order lookup, and checkout endpoints.
- `Program.cs` registers interfaces and implementations with dependency injection and starts the web application.
- `appsettings.json` contains general ASP.NET Core configuration.
- `appsettings.Development.json` contains development-specific configuration.
- `ECommerce.Application.http` can be used from Visual Studio or an HTTP client to call the API.
- `Properties/launchSettings.json` contains local development launch profiles and URLs.
- `ECommerce.Application.csproj` targets .NET 10 and references all other projects.

### Repository and solution files

- `ECommerce.Application.slnx` is the Visual Studio solution file.
- `.gitignore` excludes build output, IDE files, local secrets, logs, and test results.
- `README.md` documents the solution and usage.

## SOLID principles in this project

### Single Responsibility Principle

Each class has one main reason to change. `OrderService` handles booking orchestration, `InMemoryOrderRepository` handles persistence, payment processors handle payments, and notification services handle notifications.

### Open/Closed Principle

`CheckoutService` is closed for modification when a new payment provider is added. Create another `IPaymentProcessor` implementation and register it in `Program.cs`.

### Liskov Substitution Principle

Every payment processor returns the same `PaymentResult` contract and can be substituted without changing checkout logic.

### Interface Segregation Principle

Payment, order persistence, notification, and invoice responsibilities are represented by separate interfaces. Clients depend only on the capability they use.

### Dependency Inversion Principle

Application services depend on `IOrderRepository`, `IPaymentProcessor`, `INotificationService`, and `IInvoiceService`, not on infrastructure classes.

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 or another .NET-compatible IDE
- Optional: an HTTP client such as Visual Studio's `.http` editor, Postman, or curl

Check the SDK:

```powershell
dotnet --version
```

## Run the application

From the repository root:

```powershell
dotnet restore
dotnet build
dotnet run --project .\ECommerce.Application.csproj
```

Use the HTTPS or HTTP URL printed by ASP.NET Core. In Visual Studio, open `ECommerce.Application.slnx`, set `ECommerce.Application` as the startup project, and press `F5`.

OpenAPI is mapped automatically in development. The exact URL depends on `Properties/launchSettings.json` and the selected launch profile.

## API usage

### Book a vehicle

```http
POST /api/orders/book
Content-Type: application/json

{
  "customerName": "Asha Sharma",
  "email": "asha@example.com",
  "phoneNumber": "+1-555-0100",
  "vehicleName": "Electric SUV",
  "vehicleIdentificationNumber": "VIN123456789",
  "price": 45000
}
```

The response contains the new `orderId`, status, and total amount. Save the `orderId` for the next requests.

### Get an order

```http
GET /api/orders/{orderId}
```

### Checkout an order

```http
POST /api/orders/{orderId}/checkout
```

A successful response contains the order ID and generated invoice number. The current implementation also writes simulated email and SMS messages to the application console.

## How to change integrations

### Use card payments

Replace the UPI registration in `Program.cs`:

```csharp
builder.Services.AddSingleton<IPaymentProcessor, CardPaymentProcessor>();
```

In a production system, replace both simulated processors with an adapter for a real provider. Keep provider-specific code inside Infrastructure and keep `CheckoutService` unchanged.

### Add a new payment provider

1. Create a class in `ECommerce.Infrastructure/Payments`.
2. Implement `IPaymentProcessor`.
3. Return a `PaymentResult` from the provider call.
4. Register the implementation in `Program.cs`.

### Replace in-memory persistence

1. Add a database project or database implementation under Infrastructure.
2. Implement `IOrderRepository`.
3. Register the database repository instead of `InMemoryOrderRepository`.
4. Add connection configuration to environment variables or user secrets, not committed configuration files.

### Connect real email, SMS, and invoices

Replace the `Console.WriteLine` and generated invoice number implementations with provider adapters. Keep provider SDK references in Infrastructure. The application services should continue using the existing interfaces.

## Important current limitations

- Orders disappear when the application stops.
- A vehicle is marked unavailable during booking, with no reservation expiry or rollback workflow.
- Payment processors always succeed and do not call a real gateway.
- Email and SMS services only write to the console.
- There is no authentication, authorization, input validation policy, inventory service, or production error-handling middleware.
- The API currently uses a single configured payment processor.

These are intentional simplifications for demonstrating architecture and SOLID design.

## Recommended production improvements

1. Add EF Core and a transactional database repository.
2. Add request validation and domain-specific exceptions.
3. Add authentication and authorization.
4. Add real payment gateway integration with idempotency keys and webhook handling.
5. Add an inventory/reservation workflow with expiration.
6. Queue notifications using a background message broker.
7. Add structured logging, health checks, metrics, and distributed tracing.
8. Add unit and integration tests for booking, payment failures, and notification failures.
9. Store secrets with user secrets, a secret manager, or environment variables.
10. Add CI validation for restore, build, tests, and security scanning.

## Git commands

```powershell
git status
git add .
git commit -m "Document the e-commerce architecture"
git push origin main
```
