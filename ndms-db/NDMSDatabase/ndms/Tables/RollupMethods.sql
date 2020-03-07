CREATE TABLE [ndms].[RollupMethods]
(
	[ID]       INT          NOT NULL,
    [Name]	   NVARCHAR(100) NOT NULL, 
    [IsActive] BIT          NOT NULL,
	CONSTRAINT [PK_ndms.RollupMethods] PRIMARY KEY CLUSTERED ([ID] ASC)
)
