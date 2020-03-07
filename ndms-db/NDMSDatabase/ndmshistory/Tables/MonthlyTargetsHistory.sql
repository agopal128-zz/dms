CREATE TABLE [ndmshistory].[MonthlyTargetsHistory] (
    [ID]               INT             IDENTITY (1, 1) NOT NULL,
    [MonthlyTargetID]  INT             NOT NULL,
	[TargetID]		   INT             NOT NULL,
    [Month]            INT             NOT NULL,
    [DailyRate]			DECIMAL (18, 2) NULL,
    [MaxGoalValue]		DECIMAL (18, 2) NULL,
	[RolledUpGoalValue] DECIMAL (18, 2)	NULL,
    [StretchGoalValue]	DECIMAL (18, 2) NULL,
	[CreatedOn]		   DATETIME	       NOT NULL,
    [CreatedBy]		   INT			   NOT NULL,
	[LastModifiedOn]   DATETIME	       NOT NULL,
    [LastModifiedBy]   INT			   NOT NULL,
    CONSTRAINT [PK_ndmshistory.MonthlyTargetsHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndmshistory.MonthlyTargetsHistory_ndms.MonthlyTargets_MonthlyTargetID] FOREIGN KEY ([MonthlyTargetID]) REFERENCES [ndms].[MonthlyTargets] ([ID]),
    CONSTRAINT [FK_ndmshistory.MonthlyTargetsHistory_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndmshistory.MonthlyTargetsHistory_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndmshistory.MonthlyTargetsHistory_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  );

GO
CREATE NONCLUSTERED INDEX [IX_MonthlyTargetID]
    ON [ndmshistory].[MonthlyTargetsHistory]([MonthlyTargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndmshistory].[MonthlyTargetsHistory]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndmshistory].[MonthlyTargetsHistory]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndmshistory].[MonthlyTargetsHistory]([LastModifiedBy] ASC);
