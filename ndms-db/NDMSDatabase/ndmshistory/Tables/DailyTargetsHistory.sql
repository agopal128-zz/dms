CREATE TABLE [ndmshistory].[DailyTargetsHistory]
(
    [ID]				INT             IDENTITY (1, 1) NOT NULL,
	[DailyTargetID]		INT             NOT NULL,
    [MonthlyTargetID]	INT             NOT NULL,
    [Day]				INT             NOT NULL,	
    [MaxGoalValue]		DECIMAL (18, 2) NULL,
	[RolledUpGoalValue]	DECIMAL (18, 2) NULL,
	[IsManual]			BIT				DEFAULT(0),
	[CreatedOn]			DATETIME	    NOT NULL,
    [CreatedBy]			INT				NOT NULL,
	[LastModifiedOn]	DATETIME	    NOT NULL,
    [LastModifiedBy]	INT			  NOT NULL,
    CONSTRAINT [PK_ndmshistory.DailyTargetsHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndmshistory.DailyTargetsHistory_ndms.DailyTargets_DailyTargetID] FOREIGN KEY ([DailyTargetID]) REFERENCES [ndms].[DailyTargets] ([ID]),
    CONSTRAINT [FK_ndmshistory.DailyTargetsHistory_ndms.MonthlyTargets_MonthlyTargetID] FOREIGN KEY ([MonthlyTargetID]) REFERENCES [ndms].[MonthlyTargets] ([ID]),
	CONSTRAINT [FK_ndmshistory.DailyTargetsHistory_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndmshistory.DailyTargetsHistory_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
);

GO
CREATE NONCLUSTERED INDEX [IX_DailyTargetID]
    ON [ndmshistory].[DailyTargetsHistory]([DailyTargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_MonthlyTargetID]
    ON [ndmshistory].[DailyTargetsHistory]([MonthlyTargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndmshistory].[DailyTargetsHistory]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndmshistory].[DailyTargetsHistory]([LastModifiedBy] ASC);
