CREATE TABLE [ndmshistory].[TargetsHistory] (
    [ID]                   INT             IDENTITY (1, 1) NOT NULL,
	[TargetID]             INT			   NOT NULL,
	[ScorecardID]          INT             NOT NULL,
	[KPIID]				   INT             NOT NULL,
	[MetricID]			   INT             NOT NULL,
    [MetricType]		   INT			   NOT NULL,
    [IsCascadeFromParent]  BIT             NOT NULL,
	[ParentTargetID]	   INT			   NULL,
	[IsCascaded]		   BIT             NOT NULL,
    [IsStretchGoalEnabled] BIT             NOT NULL,
	[CalendarYearID]	   INT			   NOT NULL,
    [EffectiveStartDate]   DATE            NOT NULL,
    [EffectiveEndDate]     DATE	           NOT NULL,
	[TargetEntryMethodID]     INT          NULL,
    [RollupMethodID]       INT             NULL,
	[TrackingMethodID]     INT             NOT NULL,
	[GraphPlottingMethodID]     INT        NULL,
	[MTDPerformanceTrackingID]	INT		   NULL,
	[CascadedMetricsTrackingID] INT		   NULL,
    [AnnualTarget]         DECIMAL (18, 2) NULL,
	[IsCopiedMetric]	   BIT             NOT NULL DEFAULT(0),
	[CreatedOn]            DATETIME        NOT NULL,
    [CreatedBy]			   INT             NOT NULL,
	[LastModifiedOn]       DATETIME        NOT NULL,
    [LastModifiedBy]       INT             NOT NULL,
	[IsActive]             BIT             NOT NULL DEFAULT(1),
    CONSTRAINT [PK_ndmshistory.TargetsHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
    CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.KPIs_KPIID] FOREIGN KEY ([KPIID]) REFERENCES [ndms].[KPIs] ([ID]),
    CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Metrics_MetricID] FOREIGN KEY ([MetricID]) REFERENCES [ndms].[Metrics] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Targets_ParentTargetID] FOREIGN KEY ([ParentTargetID]) REFERENCES [ndms].[Targets] ([ID]),
    CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Years_CalendarYearID] FOREIGN KEY ([CalendarYearID]) REFERENCES [ndms].[Years] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.TargetEntryMethods_TargetEntryMethodID] FOREIGN KEY ([TargetEntryMethodID]) REFERENCES [ndms].[TargetEntryMethods] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.RollupMethods_RollupMethodID] FOREIGN KEY ([RollupMethodID]) REFERENCES [ndms].[RollupMethods] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.TrackingMethods_TrackingMethodID] FOREIGN KEY ([TrackingMethodID]) REFERENCES [ndms].[TrackingMethods] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.GraphPlottingMethods_GraphPlottingMethodID] FOREIGN KEY ([GraphPlottingMethodID]) REFERENCES [ndms].[GraphPlottingMethods] ([ID]),
    CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndmshistory.TargetsHistory_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
);

GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndmshistory].[TargetsHistory]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_ScorecardID]
    ON [ndmshistory].[TargetsHistory]([ScorecardID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_KPIID]
    ON [ndmshistory].[TargetsHistory]([KPIID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_MetricID]
    ON [ndmshistory].[TargetsHistory]([MetricID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CalendarYearID]
    ON [ndmshistory].[TargetsHistory]([CalendarYearID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_RollupMethodID]
    ON [ndmshistory].[TargetsHistory]([RollupMethodID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndmshistory].[TargetsHistory]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndmshistory].[TargetsHistory]([LastModifiedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_ParentTargetID]
    ON [ndmshistory].[TargetsHistory]([ParentTargetID] ASC);

