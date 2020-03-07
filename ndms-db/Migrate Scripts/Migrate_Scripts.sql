-- Copy existing holidays from Holidays table
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT 1,  [ndms].[Holidays].[Date], 0, 0, 1
FROM [ndms].[Holidays]
WHERE [ndms].[Holidays].[Name] NOT IN ('Saturday', 'Sunday') AND [ndms].[Holidays].[Date] < '2017-03-01'
AND NOT EXISTS (SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [ndms].[HolidayPatternInfo].[Date] = [ndms].[Holidays].[Date])

GO

-- SET the default holiday pattern for existing scorecards
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT [ndms].[Scorecards].[ID], 1, '2016-01-01', 0, 0
FROM [ndms].[Scorecards]
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] 
WHERE [ndms].[ScorecardHolidayPatterns].[ScorecardID] = [ndms].[Scorecards].[ID] 
AND [ndms].[ScorecardHolidayPatterns].[HolidayPatternID] = 1)

GO

INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate],
[EffectiveEndDate],[CreatedBy],[LastModifiedBy])
SELECT [ndms].[Scorecards].[ID], 0, 1, 1, 1, 1, 1, 0, '2016-01-01', '2017-02-28', 0, 0
FROM [ndms].[Scorecards] WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] 
WHERE [ndms].[ScorecardWorkdayPatterns].[ScorecardID] = [ndms].[Scorecards].[ID])

GO

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'ndms' 
                 AND  TABLE_NAME = 'Holidays'))
BEGIN
	DROP TABLE [ndms].[Holidays]
END
GO


UPDATE [ndms].[ScorecardHolidayPatterns]
SET EffectiveEndDate = '2017-02-28'
WHERE ScorecardID NOT IN (1,2,62)

GO 