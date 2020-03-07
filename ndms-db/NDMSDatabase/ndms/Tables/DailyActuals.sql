CREATE TABLE [ndms].[DailyActuals]
(
	[ID]	              INT             IDENTITY (1, 1) NOT NULL,
	[TargetID]	          INT			  NOT NULL,
	[Date]	              DATE			  NOT NULL,
	[ActualValue]	      DECIMAL (18, 2) NULL,
	[GoalValue]	          DECIMAL (18, 2) NULL,
	[Status]              INT             NOT NULL,
	[CreatedOn]           DATETIME        NOT NULL,
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL,
    [LastModifiedBy]      INT             NOT NULL,
	CONSTRAINT [PK_ndms.DailyActuals] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.DailyActuals_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.DailyActuals_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.DailyActuals_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
);

GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndms].[DailyActuals]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[DailyActuals]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[DailyActuals]([LastModifiedBy] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TargetID_Date]
    ON [ndms].[DailyActuals]([TargetID] ASC, [Date] ASC);

