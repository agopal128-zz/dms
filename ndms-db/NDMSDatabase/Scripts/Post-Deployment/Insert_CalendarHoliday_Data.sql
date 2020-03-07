/*Script to insert holidays in a year here */

print 'Inserting seed data for Holidays'

DECLARE @Year			 INT,
		@FirstDateOfYear DATETIME,
		@LastDateOfYear  DATETIME,
		@yearID			 INT

-- You can change @year to any year you desire
SELECT @year = 2016
SELECT @FirstDateOfYear = DATEADD(yyyy, @Year - 1900, 0)
SELECT @LastDateOfYear = DATEADD(yyyy, @Year - 1900 + 1, 0)
SELECT @yearID = ID FROM [ndms].[Years] WHERE NAME = CONVERT(varchar(10), @year)
-- Creating Query to Prepare holidays (Saturday and Sunday) for year 2017
;WITH cte AS (
SELECT 1 AS DayID,
@FirstDateOfYear AS FromDate,
DATENAME(dw, @FirstDateOfYear) AS Dayname
UNION ALL
SELECT cte.DayID + 1 AS DayID,
DATEADD(d, 1 ,cte.FromDate),
DATENAME(dw, DATEADD(d, 1 ,cte.FromDate)) AS Dayname
FROM cte
WHERE DATEADD(d,1,cte.FromDate) < @LastDateOfYear
)
INSERT INTO [ndms].[Holidays] SELECT Dayname, FromDate AS Date, @yearID
FROM CTE
WHERE DayName IN ('Saturday','Sunday') AND NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE [Date] = FromDate AND YearID = @yearID)
OPTION (MaxRecursion 370)

INSERT INTO [ndms].[Holidays] SELECT N'New Year''s Day', '01-01-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'New Year''s Day' AND YearID = @yearID ) OR [Date]='01-01-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Martin Luther King Day', '01-18-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Martin Luther King Day' AND YearID = @yearID ) OR [Date]='01-18-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Presidents'' Day', '02-15-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Presidents'' Day' AND YearID = @yearID ) OR [Date]='02-15-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Memorial Day', '05-30-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Memorial Day' AND YearID = @yearID ) OR [Date]='05-30-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Independence Day', '07-04-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Independence Day' AND YearID = @yearID ) OR [Date]='07-04-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Labor Day', '09-05-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Labor Day' AND YearID = @yearID ) OR [Date]='09-05-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Columbus Day', '10-10-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Columbus Day' AND YearID = @yearID ) OR [Date]='10-10-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Veterans Day', '11-11-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Veterans Day' AND YearID = @yearID ) OR [Date]='11-11-2016') 

INSERT INTO [ndms].[Holidays] SELECT N'Thanksgiving Day', '11-24-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Thanksgiving Day' AND YearID = @yearID ) OR [Date]='11-24-2016')
	
INSERT INTO [ndms].[Holidays] SELECT N'Christmas Day observed', '12-26-2016', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Christmas Day observed' AND YearID = @yearID ) OR [Date]='12-26-2016') 



SELECT @year = 2017
SELECT @FirstDateOfYear = DATEADD(yyyy, @Year - 1900, 0)
SELECT @LastDateOfYear = DATEADD(yyyy, @Year - 1900 + 1, 0)
SELECT @yearID = ID FROM [ndms].[Years] WHERE NAME = CONVERT(varchar(10), @Year)

-- Creating Query to Prepare holidays (Saturday and Sunday) for year 2017
;WITH cte AS (
SELECT 1 AS DayID,
@FirstDateOfYear AS FromDate,
DATENAME(dw, @FirstDateOfYear) AS Dayname
UNION ALL
SELECT cte.DayID + 1 AS DayID,
DATEADD(d, 1 ,cte.FromDate),
DATENAME(dw, DATEADD(d, 1 ,cte.FromDate)) AS Dayname
FROM cte
WHERE DATEADD(d,1,cte.FromDate) < @LastDateOfYear
)
INSERT INTO [ndms].[Holidays] SELECT Dayname, FromDate AS Date, @yearID
FROM CTE
WHERE DayName IN ('Saturday','Sunday') AND NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE [Date] = FromDate AND YearID = @yearID)
OPTION (MaxRecursion 370)	


INSERT INTO [ndms].[Holidays] SELECT N'New Year''s Day', '01-01-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'New Year''s Day' AND YearID = @yearID ) OR [Date]='01-01-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'New Year''s Day', '01-02-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'New Year''s Day' AND YearID = @yearID ) OR [Date]='01-02-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Martin Luther King Day', '01-16-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Martin Luther King Day' AND YearID = @yearID ) OR [Date]='01-16-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Presidents'' Day', '02-20-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Presidents'' Day' AND YearID = @yearID ) OR [Date]='02-20-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Memorial Day', '05-29-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Memorial Day' AND YearID = @yearID ) OR [Date]='05-29-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Independence Day', '07-04-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Independence Day' AND YearID = @yearID ) OR [Date]='07-04-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Labor Day', '09-04-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Labor Day' AND YearID = @yearID ) OR [Date]='09-04-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Columbus Day', '10-09-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Columbus Day' AND YearID = @yearID ) OR [Date]='10-09-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Veterans Day', '11-11-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Veterans Day' AND YearID = @yearID ) OR [Date]='11-11-2017') 

INSERT INTO [ndms].[Holidays] SELECT N'Thanksgiving Day', '11-23-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Thanksgiving Day' AND YearID = @yearID ) OR [Date]='11-23-2017')
	
INSERT INTO [ndms].[Holidays] SELECT N'Christmas Day', '12-25-2017', @yearID
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Holidays] WHERE ( Name = N'Christmas Day' AND YearID = @yearID ) OR [Date]='12-25-2017') 

