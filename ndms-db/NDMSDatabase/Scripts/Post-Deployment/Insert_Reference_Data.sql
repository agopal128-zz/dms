/*Script to insert data in look up tables here */

print 'Inserting seed data for Business Segments'
SET IDENTITY_INSERT [ndms].[BusinessSegments] ON
GO
INSERT [ndms].[BusinessSegments] ([ID], [Name], [IsActive])
	SELECT 0, N'All Business Segments', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[BusinessSegments] WHERE ID = 0) 
INSERT [ndms].[BusinessSegments] ([ID], [Name], [IsActive])
	SELECT 1, N'Wellbore Technologies', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[BusinessSegments] WHERE ID = 1) 
SET IDENTITY_INSERT [ndms].[BusinessSegments] OFF
GO

print 'Inserting seed data for Divisions'

SET IDENTITY_INSERT [ndms].[Divisions] ON
GO
INSERT [ndms].[Divisions] ([ID], [Name], [IsActive])
	SELECT 0, N'All Divisions', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Divisions] WHERE ID = 0) 
INSERT [ndms].[Divisions] ([ID], [Name], [IsActive])
	SELECT 1, N'Drilling and Intervention', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Divisions] WHERE ID = 1) 
SET IDENTITY_INSERT [ndms].[Divisions] OFF
GO
		
print 'Inserting seed data for Facilities'

SET IDENTITY_INSERT [ndms].[Facilities] ON
GO
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 0, N'All Facilities', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 0) 
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 1, N'Abu Dhabi', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 1) 
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 2, N'Belgium', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 2) 
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 3, N'Dubai', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 3) 
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 4, N'Russia', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 4)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 5, N'Saudi Arabia', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 5)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 6, N'Singapore', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 6)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 7, N'Stonehouse', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 7)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 8, N'Conroe', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 8)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 9, N'Breen', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 9)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 10, N'Canada', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 10)
INSERT [ndms].[Facilities] ([ID], [Name], [IsActive])
	SELECT 11, N'Brazil', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Facilities] WHERE ID = 11)
SET IDENTITY_INSERT [ndms].[Facilities] OFF
GO

print 'Inserting seed data for Product Lines'
SET IDENTITY_INSERT [ndms].[ProductLines] ON
GO
INSERT [ndms].[ProductLines] ([ID], [Name], [IsActive])
	SELECT 0, N'All Product Lines', 1   
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ProductLines] WHERE ID = 0) 
INSERT [ndms].[ProductLines] ([ID], [Name], [IsActive])
	SELECT 1, N'CT/AG Stators', 1 
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ProductLines] WHERE ID = 1) 
INSERT [ndms].[ProductLines] ([ID], [Name], [IsActive])
	SELECT 2, N'Conventional Stators', 1 
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ProductLines] WHERE ID = 2) 
INSERT [ndms].[ProductLines] ([ID], [Name], [IsActive])
	SELECT 3, N'ERT Stators', 1 
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ProductLines] WHERE ID = 3) 
INSERT [ndms].[ProductLines] ([ID], [Name], [IsActive])
	SELECT 4, N'Rotors', 1 
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ProductLines] WHERE ID = 4) 
SET IDENTITY_INSERT [ndms].[ProductLines] OFF
GO


print 'Inserting seed data for Departments'
SET IDENTITY_INSERT [ndms].[Departments] ON
GO
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 0, N'All Departments', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 0) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 1, N'HSE', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 1) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 2, N'Manufacturing', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 2) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 3, N'Production', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 3) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 4, N'Planning', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 4)
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 5, N'Programming', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 5) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 6, N'Purchasing', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 6)
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 7, N'Quality', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 7) 
INSERT [ndms].[Departments] ([ID], [Name], [IsActive])
	SELECT 8, N'Shipping', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Departments] WHERE ID = 8)
SET IDENTITY_INSERT [ndms].[Departments] OFF
GO


print 'Inserting seed data for Processes'
SET IDENTITY_INSERT [ndms].[Processes] ON
GO
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 0, N'All Processes', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 0)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 1, N'Burn and Pull', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 1)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 2, N'Injection', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 2)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 3, N'Waterjet', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 3)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 4, N'Machining', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 4)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 5, N'CMM', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 5)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 6, N'Polishing', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 6)
INSERT [ndms].[Processes] ([ID], [Name], [IsActive])
	SELECT 7, N'Assembly', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Processes] WHERE ID = 7)
SET IDENTITY_INSERT [ndms].[Processes] OFF
GO

print 'Inserting seed data for KPIs'
INSERT [ndms].[KPIs] SELECT 0, N'Safety', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 0)
INSERT [ndms].[KPIs] SELECT 1, N'Quality', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 1)
INSERT [ndms].[KPIs] SELECT 2, N'Delivery', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 2)
INSERT [ndms].[KPIs] SELECT 3, N'Innovation', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 3)
INSERT [ndms].[KPIs] SELECT 4, N'Cost', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 4)
INSERT [ndms].[KPIs] SELECT 5, N'People [Culture]', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 5)
INSERT [ndms].[KPIs] SELECT 6, N'Revenue', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 6)
INSERT [ndms].[KPIs] SELECT 7, N'Net Working Capital', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[KPIs] WHERE ID = 7)

print 'Inserting seed data for GoalTypes'
INSERT [ndms].[GoalTypes] SELECT 0, N'=', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[GoalTypes] WHERE ID = 0)
INSERT [ndms].[GoalTypes] SELECT 1, N'>=', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[GoalTypes] WHERE ID = 1)
INSERT [ndms].[GoalTypes] SELECT 2, N'<=', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[GoalTypes] WHERE ID = 2)

print 'Inserting seed data for DataTypes'
INSERT [ndms].[DataTypes] SELECT 0, N'Whole Number', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[DataTypes] WHERE ID = 0)
INSERT [ndms].[DataTypes] SELECT 1, N'Decimal Number', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[DataTypes] WHERE ID = 1)
INSERT [ndms].[DataTypes] SELECT 2, N'Percentage', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[DataTypes] WHERE ID = 2)
INSERT [ndms].[DataTypes] SELECT 3, N'Amount', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[DataTypes] WHERE ID = 3)

print 'Inserting seed data for RollupMethods'
INSERT [ndms].[RollupMethods] SELECT 0, N'Sum of Children', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[RollupMethods] WHERE ID = 0)
INSERT [ndms].[RollupMethods] SELECT 1, N'Average Of Children', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[RollupMethods] WHERE ID = 1)
INSERT [ndms].[RollupMethods] SELECT 3, N'Same As Child', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[RollupMethods] WHERE ID = 3)

print 'Inserting seed data for Graph Plotting Methods'
INSERT [ndms].[GraphPlottingMethods] SELECT 0, N'Actual'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[GraphPlottingMethods] WHERE ID = 0)
INSERT [ndms].[GraphPlottingMethods] SELECT 1, N'Cumulative'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[GraphPlottingMethods] WHERE ID = 1)

print 'Inserting seed data for Tracking Methods'
INSERT [ndms].[TrackingMethods] SELECT 0, N'Daily'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[TrackingMethods] WHERE ID = 0)
INSERT [ndms].[TrackingMethods] SELECT 1, N'Monthly'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[TrackingMethods] WHERE ID = 1)

print 'Inserting seed data for Counter Measure Status'
INSERT [ndms].[CounterMeasureStatus] SELECT 0, N'No Progress', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasureStatus] WHERE ID = 0)
INSERT [ndms].[CounterMeasureStatus] SELECT 1, N'Under Investigation', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasureStatus] WHERE ID = 1)
INSERT [ndms].[CounterMeasureStatus] SELECT 2, N'Identified Counter Measure', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasureStatus] WHERE ID = 2)
INSERT [ndms].[CounterMeasureStatus] SELECT 3, N'Counter Measure Implemented', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasureStatus] WHERE ID = 3)
INSERT [ndms].[CounterMeasureStatus] SELECT 4, N'Counter Measure Confirmed', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[CounterMeasureStatus] WHERE ID = 4)

print 'Inserting seed data for Years'
INSERT [ndms].[Years] SELECT 0, N'2016', '01-01-2016', '12-31-2016'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Years] WHERE Name = N'2016') 
INSERT [ndms].[Years] SELECT 1, N'2017', '01-01-2017', '12-31-2017'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Years] WHERE Name = N'2017') 

print 'Inserting seed data for Target Entry Methods'
INSERT [ndms].[TargetEntryMethods] SELECT 0, N'Daily'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[TargetEntryMethods] WHERE ID = 0)
INSERT [ndms].[TargetEntryMethods] SELECT 1, N'Monthly'
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[TargetEntryMethods] WHERE ID = 1)

print 'Inserting seed data for OrganizationalData'
INSERT [ndms].[OrganizationalData] SELECT 0, N'Business Segment', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 0) 
INSERT [ndms].[OrganizationalData] SELECT 1, N'Division', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 1) 
INSERT [ndms].[OrganizationalData] SELECT 2, N'Facility', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 2) 
INSERT [ndms].[OrganizationalData] SELECT 3, N'Product Line', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 3)
INSERT [ndms].[OrganizationalData] SELECT 4, N'Department', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 4) 
INSERT [ndms].[OrganizationalData] SELECT 5, N'Process', 1
	WHERE NOT EXISTS(SELECT 1 FROM [ndms].[OrganizationalData] WHERE ID = 5)



