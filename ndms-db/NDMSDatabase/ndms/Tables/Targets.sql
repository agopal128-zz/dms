CREATE TABLE [ndms].[Targets] (
    [ID]							INT     IDENTITY (1, 1) NOT NULL,
	[ScorecardID]					INT     NOT NULL,
	[KPIID]							INT     NOT NULL,
	[MetricID]						INT     NOT NULL,
    [MetricType]					INT		NOT NULL,
    [CascadeFromParent]				BIT     NOT NULL,
	[ParentTargetID]				INT		NULL,
	[IsCascaded]					BIT     NOT NULL,
    [IsStretchGoalEnabled]			BIT     NOT NULL,
	[CalendarYearID]				INT		NOT NULL,
    [EffectiveStartDate]			DATE    NOT NULL,
    [EffectiveEndDate]				DATE	NOT NULL,
	[TargetEntryMethodID]			INT     NOT NULL DEFAULT(1),
    [RollupMethodID]				INT		NULL,
	[TrackingMethodID]				INT		NOT NULL,	
	[GraphPlottingMethodID]			INT		NULL,
	[MTDPerformanceTrackingID]		INT		NULL,
	[CascadedMetricsTrackingID]	INT		NULL,
    [AnnualTarget]         DECIMAL (18, 2)	NULL,
	[IsCopiedMetric]	   BIT				NOT NULL DEFAULT (0),
	[CreatedOn]            DATETIME			NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]			   INT             NOT NULL,
	[LastModifiedOn]       DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]       INT             NOT NULL,
	[IsActive]             BIT             NOT NULL DEFAULT(1),
    CONSTRAINT [PK_ndms.Targets] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.Targets_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
    CONSTRAINT [FK_ndms.Targets_ndms.KPIs_KPIID] FOREIGN KEY ([KPIID]) REFERENCES [ndms].[KPIs] ([ID]),
    CONSTRAINT [FK_ndms.Targets_ndms.Metrics_MetricID] FOREIGN KEY ([MetricID]) REFERENCES [ndms].[Metrics] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.Targets_ParentTargetID] FOREIGN KEY ([ParentTargetID]) REFERENCES [ndms].[Targets] ([ID]),
    CONSTRAINT [FK_ndms.Targets_ndms.Years_CalendarYearID] FOREIGN KEY ([CalendarYearID]) REFERENCES [ndms].[Years] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.TargetEntryMethods_TargetEntryMethodID] FOREIGN KEY ([TargetEntryMethodID]) REFERENCES [ndms].[TargetEntryMethods] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.RollupMethods_RollupMethodID] FOREIGN KEY ([RollupMethodID]) REFERENCES [ndms].[RollupMethods] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.TrackingMethods_TrackingMethodID] FOREIGN KEY ([TrackingMethodID]) REFERENCES [ndms].[TrackingMethods] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.GraphPlottingMethods_GraphPlottingMethodID] FOREIGN KEY ([GraphPlottingMethodID]) REFERENCES [ndms].[GraphPlottingMethods] ([ID]),
    CONSTRAINT [FK_ndms.Targets_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.Targets_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
);

GO
CREATE NONCLUSTERED INDEX [IX_ScorecardID]
    ON [ndms].[Targets]([ScorecardID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_KPIID]
    ON [ndms].[Targets]([KPIID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_MetricID]
    ON [ndms].[Targets]([MetricID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CalendarYearID]
    ON [ndms].[Targets]([CalendarYearID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_RollupMethodID]
    ON [ndms].[Targets]([RollupMethodID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[Targets]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[Targets]([LastModifiedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_ParentTargetID]
    ON [ndms].[Targets]([ParentTargetID] ASC);

