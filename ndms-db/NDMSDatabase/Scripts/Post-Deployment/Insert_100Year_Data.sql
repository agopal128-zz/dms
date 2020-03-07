DECLARE @MaxId			 INT,
		@NoOfYearsToLoad INT,
		@FirstYearToLoad INT,
		@LastYearToLoad	 INT,
		@StartDateOfYear DATETIME,
		@EndDateOfYear  DATETIME;

-- set number of years to be loaded.
SELECT @NoOfYearsToLoad = 83; 

-- get the maximum ID in 
SELECT @MaxId = 1; -- year id for 2017
SELECT @FirstYearToLoad = 2017;
SELECT @LastYearToLoad = @FirstYearToLoad + @NoOfYearsToLoad;

SELECT @StartDateOfYear = DATEADD(yyyy, @FirstYearToLoad - 1900, 0)
SELECT @EndDateOfYear = DATEADD(yyyy, @FirstYearToLoad - 1900 + 1, -1)

;WITH cte AS (
SELECT @MaxId AS YearID,
@FirstYearToLoad AS YearName,
@StartDateOfYear AS StartDate,
@EndDateOfYear AS EndDate
UNION ALL
SELECT cte.YearID + 1 AS YearID,
cte.YearName + 1 AS YearName,
DATEADD(yyyy, cte.YearName - 1900 + 1, 0) AS StartDate,
DATEADD(yyyy, cte.YearName - 1900 + 2, -1) AS EndDate
FROM cte
WHERE cte.YearName < @LastYearToLoad)
INSERT INTO [ndms].[Years] 
SELECT YearID, cast(YearName AS NVARCHAR(50)), StartDate AS DATE, EndDate AS DATE
FROM cte WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Years] WHERE ID = cte.YearID OR Name = cast(cte.YearName AS NVARCHAR(50)))