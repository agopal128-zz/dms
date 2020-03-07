CREATE TABLE [ndms].[MonthlyTargets] (
    [ID]               INT             IDENTITY (1, 1) NOT NULL,
    [TargetID]		   INT             NOT NULL,
    [Month]            INT             NOT NULL,
	[DailyRate]			DECIMAL (18, 2) NULL,
    [MaxGoalValue]		DECIMAL (18, 2) NULL,
	[RolledUpGoalValue] DECIMAL (18, 2)	NULL,
    [StretchGoalValue] DECIMAL (18, 2) NULL,
	[IsRolledUpGoal]	BIT	NOT NULL DEFAULT(0),	
	[CreatedOn]		   DATETIME	       NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		   INT			   NOT NULL,
	[LastModifiedOn]   DATETIME	       NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]   INT			   NOT NULL,
    CONSTRAINT [PK_ndms.MonthlyTargets] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.MonthlyTargets_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.MonthlyTargets_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.MonthlyTargets_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  );


GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndms].[MonthlyTargets]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[MonthlyTargets]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[MonthlyTargets]([LastModifiedBy] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TargetID_Month]
    ON [ndms].[MonthlyTargets]([TargetID] ASC, [Month] ASC);