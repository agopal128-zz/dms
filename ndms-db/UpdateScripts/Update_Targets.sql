-- Update roll up targets for Monthly tracked metrics
DECLARE @previousMonth INT;
SELECT @previousMonth = (MONTH(GETUTCDATE()) - 1); 

UPDATE ndms.MonthlyTargets
SET RolledUpGoalValue = MaxGoalValue
WHERE [Month] >= @previousMonth AND TargetID IN( SELECT ID FROM ndms.Targets 
WHERE TrackingMethodID = 1 and IsCascaded = 1 and CalendarYearID = 1);

UPDATE [ndms].[Targets]
SET [CascadedMetricsTrackingID] = 1
WHERE IsCascaded = 1 AND [CascadedMetricsTrackingID] IS NULL;

DELETE FROM [ndms].[TargetEntryMethods] WHERE [Name] IN ('Weekly', 'shift');
