CREATE TABLE [ndmshistory].[DailyActualsHistory]
(
	[ID]	              INT			  IDENTITY (1, 1) NOT NULL,
	[DailyActualID]       INT             NOT NULL,
	[TargetID]	          INT			  NOT NULL,
	[Date]	              DATE			  NOT NULL,
	[ActualValue]	      DECIMAL (18, 2) NULL,
	[GoalValue]	          DECIMAL (18, 2) NULL,
	[Status]              INT             NOT NULL,
	[CreatedOn]           DATETIME        NOT NULL,
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL,
    [LastModifiedBy]      INT             NOT NULL,
	CONSTRAINT [PK_ndmshistory.DailyActualsHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndmshistory.DailyActualsHistory_ndms.DailyActuals_DailyActualID] FOREIGN KEY ([DailyActualID]) REFERENCES [ndms].[DailyActuals] ([ID]),
	CONSTRAINT [FK_ndmshistory.DailyActualsHistory_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]),
	CONSTRAINT [FK_ndmshistory.DailyActualsHistory_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndmshistory.DailyActualsHistory_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
);
GO
CREATE NONCLUSTERED INDEX [IX_DailyActualID]
    ON [ndmshistory].[DailyActualsHistory]([DailyActualID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndmshistory].[DailyActualsHistory]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndmshistory].[DailyActualsHistory]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndmshistory].[DailyActualsHistory]([LastModifiedBy] ASC);

