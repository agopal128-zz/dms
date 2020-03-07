WITH cteRowNum AS (
    SELECT SortOrder,
           ROW_NUMBER() OVER(PARTITION BY [ParentScorecardID] ORDER BY [ID]) AS RowNum
        FROM ndms.Scorecards
)

UPDATE cteRowNum
   SET SortOrder = RowNum;