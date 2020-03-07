IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.Scorecards_ndms.BusinessSegments_BusinessSegmentID]')
   AND parent_object_id = OBJECT_ID(N'ndms.Scorecards')
)
BEGIN
ALTER TABLE ndms.Scorecards 
DROP CONSTRAINT [FK_ndms.Scorecards_ndms.BusinessSegments_BusinessSegmentID];   
END
GO  

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.Divisions_ndms.BusinessSegments_BusinessSegmentID]')
   AND parent_object_id = OBJECT_ID(N'ndms.Divisions')
)
BEGIN
ALTER TABLE ndms.Divisions
DROP CONSTRAINT [FK_ndms.Divisions_ndms.BusinessSegments_BusinessSegmentID];   
END
GO  

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.MetricMappings_ndms.BusinessSegments_BusinessSegmentID]')
   AND parent_object_id = OBJECT_ID(N'ndms.MetricMappings')
)
BEGIN
ALTER TABLE ndms.MetricMappings
DROP CONSTRAINT [FK_ndms.MetricMappings_ndms.BusinessSegments_BusinessSegmentID];   
END
GO 

UPDATE [ndms].[BusinessSegments]
SET [ID] = [ID] + 1
GO
