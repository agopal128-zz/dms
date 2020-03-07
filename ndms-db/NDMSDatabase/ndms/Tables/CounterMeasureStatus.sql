CREATE TABLE [ndms].[CounterMeasureStatus]
(
	[ID]	   INT		    NOT NULL, 
    [Status]   NVARCHAR(100) NOT NULL,
	[IsActive] BIT          NOT NULL,
	CONSTRAINT [PK_ndms.[CounterMeasureStatus] PRIMARY KEY CLUSTERED ([ID] ASC)
)
