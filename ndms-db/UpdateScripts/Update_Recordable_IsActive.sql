-- Clear all the recordable date for a scorecard
-- @AdminId: UserId of NDMS admin user
DECLARE @AdminId INT = 0;
-- @ScorecardId: Scorecard Id, replace with the required scorecardID
DECLARE @ScorecardId INT = 4;

-- 
Update  [ndms].[Recordables]
set IsActive = 0,
LastModifiedOn = GETDATE(),
LastModifiedBy = @AdminId
WHERE ScorecardID = @ScorecardId ;