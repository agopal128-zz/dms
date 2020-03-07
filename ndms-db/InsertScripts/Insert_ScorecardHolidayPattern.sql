-- -- Add new Holiday patterns for all scorecards starting from 2017-03-01

--Belguim
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 80,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 80 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 97,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 97 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 98,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 98 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 99,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 99 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 100,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 100 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 114,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 114 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 167,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 167 and [EffectiveStartDate] = '2017-03-01')
GO

--Brazil
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 84,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 84 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 96,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 96 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 104,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 104 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 105,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 105 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 106,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 106 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 107,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 107 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 108,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 108 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 109,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 109 and [EffectiveStartDate] = '2017-03-01')
GO
--USA
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 4,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 4 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 5,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 5 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 6,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 6 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 7,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 7 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 8,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 8 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 9,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 9 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 10,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 10 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 14,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 14 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 16,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 16 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 18,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 18 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 19,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 19 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 44,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 44 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 45,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 45 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 81,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 81 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 161,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 161 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 162,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 162 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 163,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 163 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 58,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 58 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 121,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 121 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 122,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 122 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 123,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 123 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 124,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 124 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 125,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 125 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 126,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 126 and [EffectiveStartDate] = '2017-03-01')
GO
--Canada
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 83,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 83 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 101,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 101 and [EffectiveStartDate] = '2017-03-01')
GO
--UAE
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 63,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 63 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 64,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 64 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 65,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 65 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 66,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 66 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 67,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 67 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 68,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 68 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 86,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 86 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 87,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 87 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 88,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 88 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 89,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 89 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 102,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 102 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 103,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 103 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 111,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 111 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 119,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 119 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 147,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 147 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 152,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 152 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 153,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 153 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 154,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 154 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 155,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 155 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 156,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 156 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 157,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 157 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 158,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 158 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 159,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 159 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 160,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 160 and [EffectiveStartDate] = '2017-03-01')
GO
--Russia
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 78,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 78 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 132,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 132 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 133,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 133 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 134,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 134 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 140,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 140 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 141,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 141 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 142,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 142 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 148,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 148 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 149,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 149 and [EffectiveStartDate] = '2017-03-01')
GO
--Singapore
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 127,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 127 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 128,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 128 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 129,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 129 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 130,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 130 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 131,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 131 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 135,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 135 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 138,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 138 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 139,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 139 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 143,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 143 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 144,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 144 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 145,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 145 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 146,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 146 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 164,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 164 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 165,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 165 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 166,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 166 and [EffectiveStartDate] = '2017-03-01')
GO
--UK
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 69,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 69 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 85,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 85 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 90,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 90 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 91,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 91 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 92,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 92 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 93,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 93 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 94,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 94 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 95,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 95 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 112,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 112 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 115,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 115 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 116,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 116 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 117,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 117 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 118,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 118 and [EffectiveStartDate] = '2017-03-01')
GO 
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 151,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 151 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 70,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 70 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 71,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 71 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 72,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 72 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 73,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 73 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 74,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 74 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 75,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 75 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 76,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 76 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 77,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 77 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 120,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 120 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 136,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 136 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 137,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 137 and [EffectiveStartDate] = '2017-03-01')
GO
INSERT INTO [ndms].[ScorecardHolidayPatterns]
([ScorecardID],[HolidayPatternID],[EffectiveStartDate],[CreatedBy],[LastModifiedBy])
SELECT 168,[HolidayPatterns].[ID], '2017-03-01', 0, 0 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Saudi Arabia'
AND NOT EXISTS (SELECT 1 FROM [ndms].[ScorecardHolidayPatterns] WHERE [ScorecardID] = 168 and [EffectiveStartDate] = '2017-03-01')
GO