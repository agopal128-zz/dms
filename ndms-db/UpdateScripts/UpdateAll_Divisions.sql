IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.Scorecards_ndms.Divisions_DivisionID]')
   AND parent_object_id = OBJECT_ID(N'ndms.Scorecards')
)
BEGIN
ALTER TABLE ndms.Scorecards 
DROP CONSTRAINT [FK_ndms.Scorecards_ndms.Divisions_DivisionID];   
END
GO  

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.Facilities_ndms.Divisions_DivisionID]')
   AND parent_object_id = OBJECT_ID(N'ndms.Facilities')
)
BEGIN
ALTER TABLE ndms.Facilities
DROP CONSTRAINT [FK_ndms.Facilities_ndms.Divisions_DivisionID];   
END
GO  

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.ProductLines_ndms.Divisions_DivisionID]')
   AND parent_object_id = OBJECT_ID(N'ndms.ProductLines')
)
BEGIN
ALTER TABLE ndms.ProductLines
DROP CONSTRAINT [FK_ndms.ProductLines_ndms.Divisions_DivisionID];   
END
GO 

IF EXISTS (SELECT * 
  FROM sys.foreign_keys 
   WHERE object_id = OBJECT_ID(N'ndms.[FK_ndms.MetricMappings_ndms.Divisions_DivisionID]')
   AND parent_object_id = OBJECT_ID(N'ndms.MetricMappings')
)
BEGIN
ALTER TABLE ndms.MetricMappings
DROP CONSTRAINT [FK_ndms.MetricMappings_ndms.Divisions_DivisionID];   
END
GO 

UPDATE [ndms].[Divisions]
SET [ID] = [ID] + 1
GO
