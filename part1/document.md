# Adventure Works

## Technologies
- Backend: ASP.NET CORE API 8, C#, Entity Framework Core, LINQ, DI, AutoMapper, Fluent Validation.
- Database: SQL SERVER.
- Tool: Git, Github, Postman.

## IDE
Visual Studio 2022, Sql Server 2022, Visual Studio Code.

This project demonstrates a simplified ordering system using clean architecture principles, based on the AdventureWorks sample database. It includes:

- Layered architecture (API, Application, Domain, Repository, Database)
- FluentValidation for input validation
- Repository + Unit of Work pattern
- Low stock alert feature based on past sales data

---

## Project Structure
```
/AdventureWorks
|-- /AdventureWorks.Api
|-- /AdventureWorks.Service
|-- /AdventureWorks.Repository
|-- /AdventureWorks.Database
|-- /AdventureWorks.Domain

### 🔹 Key Folders Explanation

| Folder                      | Description                                                                 |
|----------------------------|-----------------------------------------------------------------------------|
| `AdventureWorks.Api`       | Contains controllers, DTOs, AutoMapper configs, and input validators        |
| `AdventureWorks.Service`   | Core business logic, validation services, and application use cases         |
| `AdventureWorks.Repository`| Interfaces + EF Core implementation + Unit of Work for managing transactions|
| `AdventureWorks.Database`  | Contains entity models directly mapped to the AdventureWorks database       |
| `AdventureWorks.Domain`    | Contains domain models (e.g. Order, LowStockAlert) with no dependencies     |

## ⚙️ Technical Architecture & Practices

### 🧱 Centralized Exception Handling
- A custom **middleware** is used to globally handle exceptions across the application.
- This avoids repetitive `try-catch` blocks inside controllers or services.
- It catches unexpected errors and formats them into consistent JSON error responses.
- Example responses:
  - `400 Bad Request` — validation or client-side errors
  - `404 Not Found` — missing resources
  - `500 Internal Server Error` — unhandled server exceptions

### ✅ Request Validation with FluentValidation
- The API uses the `[ApiController]` attribute for automatic model validation.
- **FluentValidation** is integrated to validate DTOs with custom business rules.
- Invalid models are automatically blocked at the controller level, returning a structured `400` error.

```csharp
[HttpPost]
public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
{
    // Validation happens automatically
}
```

### 🔄 AutoMapper Usage
- **AutoMapper** is used to transform data between layers:
  - DTO ⇄ Domain Model
  - Domain ⇄ EF Core Entity
- This minimizes repetitive mapping code and makes the logic clean and maintainable.

```csharp
CreateMap<OrderDto, Order>();
```

### 💾 Transaction Management (Unit of Work)
- Implements the **Unit of Work** pattern to group multiple database actions into a single transaction.
- All changes are committed together with `CommitAsync()` and rolled back on failure.
- Ensures data consistency and atomic operations.

```csharp
await unitOfWork.BeginTransactionAsync();
// do multiple operations
await unitOfWork.CommitTransactionAsync();
```

### 🔌 Dependency Injection
- All services, repositories, and validators are registered via **.NET Dependency Injection**.
- Promotes testability, loose coupling, and single-responsibility principle.

```csharp
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderDtoValidator>();
```

### 📡 RESTful API Standards
- API responses follow REST conventions with meaningful status codes:
  - `200 OK` — successful GET or POST operation
  - `201 Created` — for resource creation
  - `400 Bad Request` — validation or input error
  - `404 Not Found` — resource does not exist
  - `500 Internal Server Error` — unhandled exceptions

---

## ✅ Summary
This project implements modern .NET best practices for building scalable and maintainable APIs:
- Clean separation of concerns (API → Domain → Application → Repository)
- Consistent error handling and validation
- Automated data mapping with AutoMapper
- Transactional integrity using Unit of Work
- Easy to extend and test with built-in DI
