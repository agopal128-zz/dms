-- Add new Workday patterns for all scorecards starting from 2017-03-01
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 1, 1, 1, 1, 1, 1, 1, 1, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 1 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 62, 1, 1, 1, 1, 1, 1, 1, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 62 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 80, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 80 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 97, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 97 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 98, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 98 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 99, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 99 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 100, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 100 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 114, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 114 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 167, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 167 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 84, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 84 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 96, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 96 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 104, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 104 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 105, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 105 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 106, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 106 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 107, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 107 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 108, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 108 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 109, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 109 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 4, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 4 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 5, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 5 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 6, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 6 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 7, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 7 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 8, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 8 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 9, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 9 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 10, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 10 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 14, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 14 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 16, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 16 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 18, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 18 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 19, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 19 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 44, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 44 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 45, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 45 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 81, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 81 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 161, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 161 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 162, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 162 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 163, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 163 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 83, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 83 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 101, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 101 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 58, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 58 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 121, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 121 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 122, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 122 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 123, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 123 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 124, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 124 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 125, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 125 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 126, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 126 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 2, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 2 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 78, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 78 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 132, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 132 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 133, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 133 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 134, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 134 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 140, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 140 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 141, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 141 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 142, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 142 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 148, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 148 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 149, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 149 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 79, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 79 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 127, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 127 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 128, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 128 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 129, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 129 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 130, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 130 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 131, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 131 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 135, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 135 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 138, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 138 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 139, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 139 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 143, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 143 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 144, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 144 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 145, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 145 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 146, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 146 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 164, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 164 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 165, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 165 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 166, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 166 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 69, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 69 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 85, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 85 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 90, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 90 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 91, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 91 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 92, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 92 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 93, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 93 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 94, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 94 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 95, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 95 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 112, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 112 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 115, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 115 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 116, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 116 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 117, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 117 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 118, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 118 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 151, 0, 1, 1, 1, 1, 1, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 151 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 70, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 70 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 71, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 71 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 72, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 72 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 73, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 73 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 74, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 74 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 75, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 75 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 76, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 76 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 77, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 77 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 120, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 120 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 136, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 136 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 137, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 137 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 168, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 168 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 63, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 63 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 64, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 64 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 65, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 65 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 66, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 66 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 67, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 67 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 68, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 68 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 86, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 86 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 87, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 87 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 88, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 88 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 89, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 89 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 102, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 102 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 103, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 103 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 111, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 111 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 119, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 119 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 147, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 147 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 152, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 152 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 153, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 153 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 154, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 154 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 155, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 155 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 156, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 156 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 157, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 157 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 158, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 158 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 159, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 159 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardWorkdayPatterns]
([ScorecardID],[IsSunday],[IsMonday],[IsTuesday],[IsWednesday],[IsThursday],[IsFriday],[IsSaturday],[EffectiveStartDate]
,[CreatedBy],[LastModifiedBy])
SELECT 160, 1, 1, 1, 1, 1, 0, 0, '2017-03-01', 0, 0
WHERE NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardWorkdayPatterns] WHERE [ScorecardID] = 160 and [EffectiveStartDate] = '2017-03-01')
GO