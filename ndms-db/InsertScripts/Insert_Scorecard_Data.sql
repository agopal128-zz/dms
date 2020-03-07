--/*Script to load 35 pre-defined score cards here */
print 'Inserting seed data for Score cards'
SET IDENTITY_INSERT [ndms].[Scorecards] ON

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 1, N'Sr VP Mfg & Supply Chain Excellence', NULL, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Sr VP Mfg & Supply Chain Excellence');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 2, N'VP - Western Hemisphere', 1, 0, 0, 0, 0, 0, 0, 1, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'VP - Western Hemisphere');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 3, N'GM Dubai', 1, 0, 0, 3, 0, 0, 0, 1, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'GM Dubai');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 4, N'GM Breen', 2, 0, 0, 9, 0, 0, 0, 1, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'GM Breen');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 5, N'Production Manager', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Production Manager');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 6, N'Manufacturing Engineering', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Manufacturing Engineering');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 7, N'HSE Coordinator', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'HSE Coordinator');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 8, N'Quality Manager', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Quality Manager');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 9, N'Maintenance Manager', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Maintenance Manager');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 10, N'Buyer [ERT & CT/AG]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Buyer [ERT & CT/AG]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 11, N'Buyer [Power Sections and Rotors]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Buyer [Power Sections and Rotors]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 12, N'Planner [Rotors]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Planner [Rotors]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 13, N'Planner [CT/AG]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Planner [CT/AG]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 14, N'Planner [ERT]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Planner [ERT]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 15, N'Planner [Power Sections]', 4, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Planner [Power Sections]');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 16, N'Production Supervisor (Forch)', 5, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Production Supervisor (Forch)');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 17, N'Production Supervisor (Womack)', 5, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Production Supervisor (Womack)');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 18, N'Production Supervisor 3', 5, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Production Supervisor 3');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 19, N'Production Supervisor (Harvey)', 5, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Production Supervisor (Harvey)');

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 20, N'Team Lead 1 [ERT]', 16, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 1 [ERT]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 21, N'Team Lead 4 [Power Sections]', 16, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 4 [Power Sections]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 22, N'Team Lead 7 [CT/AG]', 16, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 7 [CT/AG]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 23, N'Team Lead 2 [ERT]', 17, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 2 [ERT]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 24, N'Team Lead 5 [Power Sections]', 17, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 5 [Power Sections]')


INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 25, N'Team Lead 8 [CT/AG]', 17, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 8 [CT/AG]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 26, N'Team Lead 3 [ERT]', 18, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 3 [ERT]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 27, N'Team Lead 6 [Power Sections]', 18, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 6 [Power Sections]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 28, N'Team Lead 9 [CT/AG]', 18, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 9 [CT/AG]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 29, N'Team Lead 10 [Rotors]', 19, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 10 [Rotors]')


INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 30, N'Team Lead 11 [Rotors]', 19, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 11 [Rotors]')

INSERT INTO [ndms].[Scorecards]([ID],
    [Name], [ParentScorecardID], [BusinessSegmentID], [DivisionID], [FacilityID],
    [ProductLineID], [DepartmentID], [ProcessID], [IsBowlingChartApplicable],
    [DrilldownLevel], [CreatedBy], [LastModifiedBy]) 
SELECT 31, N'Team Lead 12 [Rotors]', 19, 0, 0, 9, 0, 0, 0, 0, 2, 0, 0
WHERE NOT EXISTS(SELECT 1 FROM [ndms].Scorecards WHERE Name = N'Team Lead 12 [Rotors]')

SET IDENTITY_INSERT [ndms].[Scorecards] OFF

--/*Script to add all kpis to scorecard */

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 1, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 1 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 2, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 2 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 3, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 3 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 4, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 4 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 5, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 5 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 6, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 6 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 7, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 7 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 8, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 8 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 9, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 9 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 10, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 10 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 11, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 11 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 12, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 12 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 13, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 13 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 14, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 14 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 15, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 15 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 16, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 16 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 17, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 17 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 18, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 18 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 19, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 19 AND KPIRefId = 5)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 20, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 20 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 21, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 21 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 22, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 22 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 23, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 23 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 24, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 24 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 25, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 25 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 26, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 26 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 27, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 27 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 28, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 28 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 29, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 29 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 30, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 30 AND KPIRefId = 5)

INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 0 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 0)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 1 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 1)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 2 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 2)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 3 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 3)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 4 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 4)
INSERT INTO [ndms].[ScorecardKPI]([ScorecardRefId],[KPIRefId])
     SELECT 31, 5 
WHERE NOT EXISTS(SELECT 1 FROM [ndms].[ScorecardKPI] WHERE ScorecardRefId = 31 AND KPIRefId = 5)

GO
