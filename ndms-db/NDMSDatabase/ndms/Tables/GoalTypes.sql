CREATE TABLE [ndms].[GoalTypes]
(
	[ID]       INT            NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
	[IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_ndms.GoalTypes] PRIMARY KEY CLUSTERED ([ID] ASC)
)

