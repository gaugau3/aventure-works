# ASP.NET Developer Interview Test

This repository contains my solution to the ASP.NET Developer Interview Test. The test is split into two main parts:

## 📁 Structure

├── part1/ # ASP.NET Core Web API (EF Core, DTO, LINQ, DI)
├── part2/ # SQL Scripts (Stored Procedures, Functions)
└── .gitignore

## 🧩 Part 1: ASP.NET Core (.NET 8 & EF Core)

### 1. Low Stock Alert API
- **Endpoint**: `/inventories/low-stock-alerts`
- **Logic**: Calculates average quantity sold in the last 3 months per product. Triggers alert if:
current_stock < 2 * avg_sold_last_3_months

- **Technologies**: EF Core, LINQ, DTO, Exception Handling, HTTP Status Codes.

### 2. Monthly Report API
- **Endpoint**: `/reports/monthly-sales`
- **Features**:
- Total orders & amount
- Top-selling product
- Growth rate compared to previous month
- **Technologies**: LINQ, DTOs, In-memory caching (IMemoryCache), null handling

### 3. Service Layer & DI
- Implemented `IOrderService` with:
- `CreateOrderAsync`
- `GetOrdersByCustomerAsync`
- `UpdateOrderStatusAsync`
- **Extras**: Scoped lifetime, transactions, exception handling

---

## 🧮 Part 2: SQL Server – AdventureWorks

### 1. Yearly Revenue Growth
- **Stored Procedure**: `uspGetYearlyRevenueGrowth`
- Filters by year range and optionally product category
- Calculates `TotalRevenue` and `Year-over-Year Growth`

### 2. Cross-Sell Recommendations
- **Stored Procedure**: `uspGetCrossSellRecommendations`
- Based on similar customers' history
- Inputs: `@CustomerID`, `@TopN`
- Output: ProductID, Name, Recommendation Score

### 3. Repeat Purchase Analysis
- **Function**: `ufnGetRepeatPurchaseGapBySubCategory`
- Input: `CustomerID`
- Output:
- ProductSubCategory
- Purchase Count
- Average days between purchases

---

## ✅ How to Run

1. Clone the repository
2. Restore the **AdventureWorks** database
3. Run `part1` ASP.NET Core API in Visual Studio or CLI
4. Run SQL scripts in `part2` using SSMS

---

## 💡 Notes

- Code follows clean architecture principles
- DTO and Service layers used for separation of concerns
- Includes basic error handling and logging
- All SQL procedures are tested on AdventureWorks DB

---

## 📌 Author

[Tran_Ngoc_Chi] | Contact via email upon request
