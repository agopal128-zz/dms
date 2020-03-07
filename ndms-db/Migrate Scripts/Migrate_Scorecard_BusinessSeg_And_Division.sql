UPDATE [ndms].Scorecards
SET BusinessSegmentID = BusinessSegmentID + 1;

UPDATE [ndms].Scorecards
SET DivisionID = DivisionID + 1;

UPDATE [ndms].[MetricMappings]
SET BusinessSegmentID = BusinessSegmentID + 1;

UPDATE [ndms].[MetricMappings]
SET DivisionID = DivisionID + 1;

-- INSERT BUSINESS SEGMENTS
INSERT INTO ndms.ScorecardBusinessSegments(ScorecardRefId, BussinessSegmentRefId)
SELECT DISTINCT ID, BusinessSegmentID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardBusinessSegments WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- INSERT DIVISIONS
INSERT INTO ndms.ScorecardDivisions(ScorecardRefId, DivisionRefId)
SELECT DISTINCT ID, DivisionID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardDivisions WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- INSERT FACILITIES
INSERT INTO ndms.ScorecardFacilities(ScorecardRefId, FacilityRefId)
SELECT DISTINCT ID, FacilityID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardFacilities WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- INSERT DEPARTMENTS
INSERT INTO ndms.ScorecardDepartments(ScorecardRefId, DepartmentRefId)
SELECT DISTINCT ID, DepartmentID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardDepartments WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- INSERT PROCESSES
INSERT INTO ndms.ScorecardProcesses(ScorecardRefId, ProcessRefId)
SELECT DISTINCT ID, ProcessID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardProcesses WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- INSERT PRODUCT LINES
INSERT INTO ndms.ScorecardProductLines(ScorecardRefId, ProductLineRefId)
SELECT DISTINCT ID, ProductLineID FROM [ndms].Scorecards
WHERE NOT EXISTS(SELECT 1 FROM ndms.ScorecardProductLines WHERE ScorecardRefId = [ndms].Scorecards.ID);

-- DROP COLUMNS FROM SCORECARD
ALTER TABLE [ndms].[Scorecards]
DROP COLUMN [BusinessSegmentID],[DivisionID],[FacilityID],[ProductLineID],[DepartmentID],[ProcessID]
