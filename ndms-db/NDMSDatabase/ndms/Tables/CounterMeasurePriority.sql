CREATE TABLE [ndms].[CounterMeasurePriority]
(
	[ID]		INT				NOT NULL, 
    [Name]		NVARCHAR(100)	NOT NULL, 
    [IsActive]	BIT				NOT NULL,
	CONSTRAINT [PK_ndms.CounterMeasurePriority] PRIMARY KEY CLUSTERED ([ID] ASC),
)
