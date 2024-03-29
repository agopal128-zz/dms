-- Update the last recordable date 
-- @RecordableDate: the date in yyyy-mm-dd format, replace with required date
DECLARE @RecordableDate DATE = '2017-01-16';
-- @AdminId: UserId of NDMS admin user, replace with required userID
DECLARE @AdminId INT = 0;
-- @ScorecardId: Scorecard Id, replace with required scorecardID
DECLARE @ScorecardId INT = 5140;

Update  [ndms].[Recordables]
set RecordableDate = @RecordableDate,
LastModifiedOn = GETDATE(),
LastModifiedBy = @AdminId
WHERE ScorecardID = @ScorecardId 
AND IsManual = 1 And IsActive = 1;