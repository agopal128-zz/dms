CREATE TABLE [ndms].[CounterMeasures]
(
	[ID]								INT				IDENTITY (1, 1) NOT NULL, 
    [ScorecardID]						INT				NOT NULL, 
    [KPIID]								INT				NOT NULL, 
    [MetricID]							INT				NOT NULL, 
    [Issue]								NVARCHAR(300)	NOT NULL, 
    [Action]							NVARCHAR(300)	NOT NULL, 
	[AssignedTo]						INT				NOT NULL,
    [DueDate]							DATE			NOT NULL, 
    [CounterMeasureStatusID]			INT				NOT NULL,
	[CounterMeasurePriorityID]	INT		NULL,
	[CreatedOn]							DATETIME		NOT NULL,
    [CreatedBy]							INT				NOT NULL,
	[LastModifiedOn]					DATETIME		NOT NULL,
    [LastModifiedBy]					INT				NOT NULL,
	CONSTRAINT [PK_ndms.[CounterMeasures] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
    CONSTRAINT [FK_ndms.CounterMeasures_ndms.KPIs_KPIID] FOREIGN KEY ([KPIID]) REFERENCES [ndms].[KPIs] ([ID]),
    CONSTRAINT [FK_ndms.CounterMeasures_ndms.Metrics_MetricID] FOREIGN KEY ([MetricID]) REFERENCES [ndms].[Metrics] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.CounterMeasureStatus_CounterMeasureStatusID] FOREIGN KEY ([CounterMeasureStatusID]) REFERENCES [ndms].[CounterMeasureStatus] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.CounterMeasurePriority_CounterMeasurePriorityID] FOREIGN KEY ([CounterMeasurePriorityID]) REFERENCES [ndms].[CounterMeasurePriority] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasures_ndms.Users_AssignedTo] FOREIGN KEY ([AssignedTo]) REFERENCES [ndms].[Users] ([ID])

);

GO
CREATE NONCLUSTERED INDEX [IX_ScorecardID]
    ON [ndms].[CounterMeasures]([ScorecardID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_KPIID]
    ON [ndms].[CounterMeasures]([KPIID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_MetricID]
    ON [ndms].[CounterMeasures]([MetricID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CounterMeasureStatusID]
    ON [ndms].[CounterMeasures]([CounterMeasureStatusID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[CounterMeasures]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[CounterMeasures]([LastModifiedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_AssignedTo]
    ON [ndms].[CounterMeasures]([AssignedTo] ASC);