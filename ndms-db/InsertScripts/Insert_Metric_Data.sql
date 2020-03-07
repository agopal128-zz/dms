
/*Script to insert pre-defined metrics here */
print 'Inserting seed data for Metrics'
SET IDENTITY_INSERT [ndms].[Metrics] ON
GO
INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 0,N'# of completions',0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of completions')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 1,N'# of Late work orders', 0,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Late work orders')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 2,N'# of NCRs', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of NCRs')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 3,N'# of parts moved to Shipping', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of parts moved to Shipping')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 4,N'# of Peer to Peers', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Peer to Peers')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 5,N'# of Quality Process Audits', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Quality Process Audits')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 6,N'# of Recordables', 0,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Recordables')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 7,N'# of Value Streams Meeting Leadtime Target', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Value Streams Meeting Leadtime Target')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 8,N'# of Work Order WIP in Production', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of Work Order WIP in Production')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 9,N'# of work orders returned back to planning', 0,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of work orders returned back to planning')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 10,N'# of WOs Printed', 0,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'# of WOs Printed')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 11,N'% Pass rate from Quality Process Audits', 2,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'% Pass rate from Quality Process Audits')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 12,N'5S Score', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'5S Score')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 13,N'5-Why FPY', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'5-Why FPY')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 14,N'Absorption Dollars', 3,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Absorption Dollars')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 15,N'Average time NCRs are open', 1,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Average time NCRs are open')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 16,N'Cost of Quality Percentage', 2,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Cost of Quality Percentage')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 17,N'Daily Machine hours claimed', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Daily Machine hours claimed')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 18,N'Financial Break Even', 0,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Financial Break Even')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 19,N'Leadtime CT/AG', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Leadtime CT/AG')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 20,N'Leadtime ERT Reline', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Leadtime ERT Reline')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 21,N'Leadtime Power Section Reline', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Leadtime Power Section Reline')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 22,N'Leadtime Rotors', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Leadtime Rotors')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 23,N'On Time Delivery %', 2,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'On Time Delivery %')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 24,N'Overall Actual Plant Leadtime', 1,1,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Overall Actual Plant Leadtime')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 25,N'Right Mix', 2,0,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Right Mix')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 26,N'Scrap and Rework', 3,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'Scrap and Rework')

INSERT INTO [ndms].[Metrics]([ID],[Name],[DataTypeID],[GoalTypeID],[CreatedBy],[LastModifiedBy])
     SELECT 27,N'WO LT Open to Close time in days', 1,2,0,0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[Metrics] WHERE Name = N'WO LT Open to Close time in days')

SET IDENTITY_INSERT [ndms].[Metrics] OFF