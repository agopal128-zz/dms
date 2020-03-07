--Insert Holidays
--Belgium
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-01')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-17', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-17')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-01')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-25')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-05', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-05')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-07-21', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-07-21')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-15', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-15')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-27', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-27')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-01')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Belgium'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
--Brazil
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Brazil'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-04')
GO
--Canada
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-13', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-13')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-20', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-20')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-14', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-14')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-22', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-22')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-07-03', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-07-03')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-07', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-07')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-04')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-10-09', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-10-09')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-13', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-13')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-26')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
--Canada - Newfoundland
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-14', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-14')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-22', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-22')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-26')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-07-03', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-07-03')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-02')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-04')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-10-09', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-10-09')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-13', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-13')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-26')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Canada-Newfoundland'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
--Russia
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-03', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-03')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-04')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-05', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-05')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-06', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-06')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-07', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-07')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-08', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-08')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-23', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-23')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-24', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-24')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-03-08', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-03-08')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-29', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-29')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-30', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-30')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-08', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-08')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-09', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-09')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-10', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-10')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-11', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-11')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-12', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-12')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-04')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-05', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-05')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-06', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Russia'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-06')
GO
--Singapore
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-30', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-30')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-14', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-14')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-10', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-10')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-26')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-09', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-09')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-10-18', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-10-18')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'Singapore'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
--UAE
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-24', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-24')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-25')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-26')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-06-27', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-06-27')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-31', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-31')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-03', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-03')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-21', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-21')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-30', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-30')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-01')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UAE'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO

--UK
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-14', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-14')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-17', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-17')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-01')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-29', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-29')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-08-28', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-08-28')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-26')
GO
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'UK'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
--USA
INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-01-02', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-01-02')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-02-20', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-02-20')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-04-14', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-04-14')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-05-29', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-05-29')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-07-03', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-07-03')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-07-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-07-04')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-09-04', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-09-04')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-23', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-23')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-11-24', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-11-24')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-25', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-25')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2017-12-26', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2017-12-26')
GO

INSERT INTO [ndms].[HolidayPatternInfo]
([HolidayPatternID],[Date],[CreatedBy],[LastModifiedBy],[IsActive])
SELECT [HolidayPatterns].[ID], '2018-01-01', 0, 0, 1 FROM [ndms].[HolidayPatterns] WHERE [Name] = 'USA'
AND NOT EXISTS ( SELECT 1 FROM [ndms].[HolidayPatternInfo] WHERE [HolidayPatternID] = [HolidayPatterns].[ID] and [Date] = '2018-01-01')
GO
