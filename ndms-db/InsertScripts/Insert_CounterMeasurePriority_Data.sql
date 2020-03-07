INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 1, 'Blocker', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'Blocker');
INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 2, 'Critical', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'Critical');
INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 3, 'Major', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'Major');
INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 4, 'Minor', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'Minor');
INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 5, 'Not Prioritized', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'Not Prioritized');
INSERT INTO [ndms].[CounterMeasurePriority]
([ID], [Name], [IsActive])
SELECT 6, 'On Hold', 1
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasurePriority] WHERE Name = N'On Hold');
GO


