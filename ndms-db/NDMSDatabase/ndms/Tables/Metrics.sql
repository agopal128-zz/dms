CREATE TABLE [ndms].[Metrics] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (100) NOT NULL,
    [GoalTypeID]     INT            NOT NULL,
    [DataTypeID]     INT            NOT NULL,
	[CreatedOn]      DATETIME       NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		 INT            NOT NULL,
	[LastModifiedOn] DATETIME       NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy] INT            NOT NULL,
	[IsActive]       BIT            NOT NULL  DEFAULT(1),
    
    CONSTRAINT [PK_ndms.Metrics] PRIMARY KEY CLUSTERED ([ID] ASC), 
    CONSTRAINT [FK_ndms.Metrics_ndms.GoalTypes_GoalTypeID] FOREIGN KEY ([GoalTypeID]) REFERENCES [ndms].[GoalTypes]([ID]),
	CONSTRAINT [FK_ndms.Metrics_ndms.DataTypes_DataTypeID] FOREIGN KEY ([DataTypeID]) REFERENCES [ndms].[DataTypes]([ID]),
	CONSTRAINT [FK_ndms.Metrics_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.Metrics_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]));

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON [ndms].[Metrics]([Name] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_GoalTypeID]
    ON [ndms].[Metrics]([GoalTypeID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_DataTypeID]
    ON [ndms].[Metrics]([DataTypeID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[Metrics]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[Metrics]([LastModifiedBy] ASC);
