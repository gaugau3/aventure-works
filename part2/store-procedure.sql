-- Part 2.1
DROP PROCEDURE IF EXISTS uspGetYearlyRevenueGrowth;
GO
CREATE PROCEDURE uspGetYearlyRevenueGrowth
    @StartYear INT,
    @EndYear INT,
    @ProductCategoryID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StartDate DATE = DATEFROMPARTS(@StartYear, 1, 1);
    DECLARE @EndDate DATE = DATEFROMPARTS(@EndYear + 1, 1, 1); -- exclusive end

    ;WITH RevenueByYear AS (
        SELECT
            YEAR(soh.OrderDate) AS OrderYear,
            SUM(soh.TotalDue) AS TotalRevenue
        FROM Sales.SalesOrderHeader soh
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        INNER JOIN Production.Product p ON sod.ProductID = p.ProductID
        LEFT JOIN Production.ProductSubcategory ps ON p.ProductSubcategoryID = ps.ProductSubcategoryID
        LEFT JOIN Production.ProductCategory pc ON ps.ProductCategoryID = pc.ProductCategoryID
        WHERE soh.OrderDate >= @StartDate AND soh.OrderDate < @EndDate
          AND (@ProductCategoryID IS NULL OR pc.ProductCategoryID = @ProductCategoryID)
        GROUP BY YEAR(soh.OrderDate)
    )
    SELECT
        ry.OrderYear,
        ry.TotalRevenue,
        LAG(ry.TotalRevenue) OVER (ORDER BY ry.OrderYear) AS PrevYearRevenue,
        CASE
            WHEN LAG(ry.TotalRevenue) OVER (ORDER BY ry.OrderYear) IS NULL THEN NULL
            ELSE ROUND(100.0 * (ry.TotalRevenue - LAG(ry.TotalRevenue) OVER (ORDER BY ry.OrderYear)) / 
                        LAG(ry.TotalRevenue) OVER (ORDER BY ry.OrderYear), 2)
        END AS GrowthRatePercent
    FROM RevenueByYear ry
    ORDER BY ry.OrderYear;
END;
EXEC uspGetYearlyRevenueGrowth @StartYear = 2011, @EndYear = 2014, @ProductCategoryID = 2;

-- Part 2.2
DROP PROCEDURE IF EXISTS uspGetCrossSellRecommendations;
GO
CREATE PROCEDURE uspGetCrossSellRecommendations
    @CustomerID INT,
    @TopN INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Step 1: Products that current customers have purchased
    ;WITH CustomerProducts AS (
        SELECT DISTINCT sod.ProductID
        FROM Sales.SalesOrderHeader soh
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        WHERE soh.CustomerID = @CustomerID
    ),

    -- Step 2: Other customers have purchased at least 2 similar products
    SimilarCustomers AS (
        SELECT soh.CustomerID
        FROM Sales.SalesOrderHeader soh
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        WHERE sod.ProductID IN (SELECT ProductID FROM CustomerProducts)
          AND soh.CustomerID <> @CustomerID
        GROUP BY soh.CustomerID
        HAVING COUNT(DISTINCT sod.ProductID) >= 2
    ),

    -- Step 3: Products that similar customers have purchased
    SimilarCustomerProducts AS (
        SELECT DISTINCT sc.CustomerID, sod.ProductID
        FROM SimilarCustomers sc
        INNER JOIN Sales.SalesOrderHeader soh ON soh.CustomerID = sc.CustomerID
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        WHERE sod.ProductID NOT IN (SELECT ProductID FROM CustomerProducts)
    )

    -- Step 4 + 5: Calculate RecommendationScore and get Top N
    SELECT TOP (@TopN)
        p.ProductID,
        p.Name AS ProductName,
        COUNT(DISTINCT scp.CustomerID) AS RecommendationScore
    FROM SimilarCustomerProducts scp
    INNER JOIN Production.Product p ON p.ProductID = scp.ProductID
    GROUP BY p.ProductID, p.Name
    ORDER BY RecommendationScore DESC;
END;
EXEC uspGetCrossSellRecommendations @CustomerID = 11000, @TopN = 5;

-- Part 2.3
DROP FUNCTION IF EXISTS ufnGetRepeatPurchaseGapBySubCategory;
GO
CREATE FUNCTION dbo.ufnGetRepeatPurchaseGapBySubCategory
(
    @CustomerID INT
)
RETURNS @Result TABLE
(
    ProductSubCategoryID INT,
    ProductSubCategoryName NVARCHAR(100),
    OrderCount INT,
    AvgGapDays FLOAT
)
AS
BEGIN
    ;WITH PurchaseHistory AS (
        SELECT
            ps.ProductSubcategoryID,
            ps.Name AS ProductSubCategoryName,
            soh.OrderDate,
            ROW_NUMBER() OVER (
                PARTITION BY ps.ProductSubcategoryID ORDER BY soh.OrderDate
            ) AS rn
        FROM Sales.SalesOrderHeader soh
        INNER JOIN Sales.SalesOrderDetail sod ON soh.SalesOrderID = sod.SalesOrderID
        INNER JOIN Production.Product p ON p.ProductID = sod.ProductID
        INNER JOIN Production.ProductSubcategory ps ON p.ProductSubcategoryID = ps.ProductSubcategoryID
        WHERE soh.CustomerID = @CustomerID
        GROUP BY ps.ProductSubcategoryID, ps.Name, soh.OrderDate
    ),
    Gaps AS (
        SELECT
            curr.ProductSubcategoryID,
            curr.ProductSubCategoryName,
            DATEDIFF(DAY, prev.OrderDate, curr.OrderDate) AS GapDays
        FROM PurchaseHistory curr
        INNER JOIN PurchaseHistory prev
            ON curr.ProductSubcategoryID = prev.ProductSubcategoryID
            AND curr.rn = prev.rn + 1
    )
    INSERT INTO @Result
    SELECT
        ph.ProductSubcategoryID,
        ph.ProductSubCategoryName,
        COUNT(DISTINCT ph.OrderDate) AS OrderCount,
        AVG(CAST(g.GapDays AS FLOAT)) AS AvgGapDays
    FROM PurchaseHistory ph
    LEFT JOIN Gaps g
        ON ph.ProductSubcategoryID = g.ProductSubcategoryID
    GROUP BY ph.ProductSubcategoryID, ph.ProductSubCategoryName
    HAVING COUNT(DISTINCT ph.OrderDate) >= 2;

    RETURN;
END;
SELECT * FROM dbo.ufnGetRepeatPurchaseGapBySubCategory(11000);
