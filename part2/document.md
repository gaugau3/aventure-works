# 📊 SQL Data Analysis Procedures & Function

This submission includes solutions to 3 business data analysis tasks using SQL Server. Each solution is designed based on performance, clarity, and business alignment.

---

## 🔹 Part 1: Yearly Revenue Growth Analysis

### 📌 Procedure: `uspGetYearlyRevenueGrowth`

#### ✅ Business Requirements:
- Calculate total revenue per year (`TotalDue`) within a given year range.
- Filter by product category (optional).
- Compute year-over-year growth rate.

#### 🔍 Logic:
1. **Parameters**: `@StartYear`, `@EndYear`, `@ProductCategoryID (optional)`
2. **Date Range**: From Jan 1 of `@StartYear` to Jan 1 of `@EndYear + 1` (exclusive upper bound).
3. **Joins**:
   - Link `SalesOrderHeader` → `SalesOrderDetail` → `Product` → `ProductSubcategory` → `ProductCategory`.
4. **Filtering**:
   - Match order dates within the range.
   - Filter by product category if provided.
5. **Revenue Calculation**: Group by year, aggregate `TotalDue`.
6. **Growth Rate**:
   - Use `LAG()` window function to get the previous year's revenue.
   - Calculate growth as a percentage difference.

#### 🛠 Use Case:
Ideal for business analysts and reporting tools to monitor revenue trends annually.

---

## 🔹 Part 2: Cross-Sell Product Recommendations

### 📌 Procedure: `uspGetCrossSellRecommendations`

#### ✅ Business Requirements:
- Input: `CustomerID`, `TopN`
- Recommend products the customer hasn't bought but were bought by other similar customers.

#### 🔍 Logic:
1. **CustomerProducts**: All products the target customer has purchased.
2. **SimilarCustomers**: Other customers who purchased at least 2 of the same products.
3. **SimilarCustomerProducts**: Products bought by those similar customers that the target customer hasn’t purchased.
4. **RecommendationScore**: Count how many similar customers bought each new product.
5. **Result**: Top N products with the highest score.

#### 🛠 Use Case:
Used in e-commerce or CRM platforms to provide smart product suggestions, increasing upsell opportunities.

---

## 🔹 Part 3: Repeat Purchase Gap Analysis

### 📌 Function: `ufnGetRepeatPurchaseGapBySubCategory`

#### ✅ Business Requirements:
- Input: `CustomerID`
- For each product subcategory the customer has purchased from at least 2 times:
  - Count how many orders.
  - Calculate the average number of days between consecutive purchases.

#### 🔍 Logic:
1. **PurchaseHistory CTE**:
   - List of all subcategory purchases with order dates and row numbers by subcategory.
2. **Gaps CTE**:
   - Calculate day difference between consecutive purchases using `DATEDIFF()` and `ROW_NUMBER()`.
3. **Final Aggregation**:
   - Group by subcategory.
   - Include only those with 2+ purchases.
   - Return: Subcategory ID, name, order count, average gap in days.

#### 🛠 Use Case:
Useful for analyzing customer loyalty and repeat behavior. Can guide re-engagement campaigns.

---

## ✅ Sample Execution

```sql
-- Part 1
EXEC uspGetYearlyRevenueGrowth @StartYear = 2011, @EndYear = 2014, @ProductCategoryID = 2;

-- Part 2
EXEC uspGetCrossSellRecommendations @CustomerID = 11000, @TopN = 5;

-- Part 3
SELECT * FROM dbo.ufnGetRepeatPurchaseGapBySubCategory(11000);
