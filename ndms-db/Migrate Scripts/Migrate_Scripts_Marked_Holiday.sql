DECLARE @MarkedHolidays Table
(
	[ScorecardID] INT,
	[Date] DATE
)

DECLARE @UnmarkedHolidays Table
(
	[ScorecardID] INT,
	[Date] DATE
)

INSERT INTO @MarkedHolidays([ScorecardID],[Date])
(SELECT DISTINCT [ndms].[Targets].[ScorecardID], [ndms].[DailyActuals].[Date] 
FROM [ndms].[DailyActuals] 
JOIN [ndms].[Targets] ON [ndms].[Targets].[ID] = [ndms].[DailyActuals].[TargetID]
WHERE [ndms].[DailyActuals].[Status] = 3);

INSERT INTO @UnmarkedHolidays([ScorecardID],[Date])
(SELECT DISTINCT [ndms].[Targets].[ScorecardID], [ndms].[DailyActuals].[Date] 
FROM [ndms].[DailyActuals] 
JOIN [ndms].[Targets] ON [ndms].[Targets].[ID] = [ndms].[DailyActuals].[TargetID]
WHERE [ndms].[DailyActuals].[Status] = 0);

-- insert marked non-workdays in new table
INSERT INTO [ndms].[ScorecardWorkdayTracker]
([ScorecardID],[Date],[IsWorkDay],[CreatedBy],[LastModifiedBy],[IsActive])
(SELECT [ScorecardID], [Date], 0,0,0,1
FROM @MarkedHolidays holidays WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardWorkdayTracker] 
WHERE [ndms].[ScorecardWorkdayTracker].ScorecardID = holidays.ScorecardID 
AND [ndms].[ScorecardWorkdayTracker].[Date] = holidays.[Date]));

-- insert marked workdays in new table
INSERT INTO [ndms].[ScorecardWorkdayTracker]
([ScorecardID],[Date],[IsWorkDay],[CreatedBy],[LastModifiedBy],[IsActive])
(SELECT [ScorecardID], [Date], 1,0,0,1
FROM @UnmarkedHolidays workdays WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardWorkdayTracker] 
WHERE [ndms].[ScorecardWorkdayTracker].ScorecardID = workdays.ScorecardID 
AND [ndms].[ScorecardWorkdayTracker].[Date] = workdays.[Date]));

-- delete holiday markings from actual table
DELETE FROM [ndmshistory].[DailyActualsHistory] WHERE DailyActualID IN 
(SELECT ID FROM [ndms].[DailyActuals] WHERE [Status] = 3);
DELETE FROM [ndms].[DailyActuals] WHERE [Status] = 3;

