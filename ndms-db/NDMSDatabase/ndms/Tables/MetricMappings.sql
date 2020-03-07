CREATE TABLE [ndms].[MetricMappings] (
    [ID]                INT IDENTITY (1, 1) NOT NULL,
    [BusinessSegmentID] INT		 NOT NULL,
    [DivisionID]        INT		 NOT NULL,
    [FacilityID]        INT		 NOT NULL,
	[ProductLineID]     INT		 NOT NULL,
    [DepartmentID]      INT		 NOT NULL,
    [ProcessID]         INT		 NOT NULL,
	[KPIID]             INT		 NOT NULL,
    [MetricID]          INT		 NOT NULL,
	[CreatedOn]			DATETIME NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]			INT      NOT NULL,
	[LastModifiedOn]	DATETIME NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]	INT      NOT NULL,
	[IsActive]         BIT       NOT NULL DEFAULT(1),
    
    CONSTRAINT [PK_ndms.MetricMappings] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.BusinessSegments_BusinessSegmentID] FOREIGN KEY ([BusinessSegmentID]) REFERENCES [ndms].[BusinessSegments] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Divisions_DivisionID] FOREIGN KEY ([DivisionID]) REFERENCES [ndms].[Divisions] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Facilities_FacilityID] FOREIGN KEY ([FacilityID]) REFERENCES [ndms].[Facilities] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.ProductLines_ProductLineID] FOREIGN KEY ([ProductLineID]) REFERENCES [ndms].[ProductLines] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Departments_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [ndms].[Departments] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Processes_ProcessID] FOREIGN KEY ([ProcessID]) REFERENCES [ndms].[Processes] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.KPIs_KPIID] FOREIGN KEY ([KPIID]) REFERENCES [ndms].[KPIs] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Metrics_MetricID] FOREIGN KEY ([MetricID]) REFERENCES [ndms].[Metrics] ([ID]),
    CONSTRAINT [FK_ndms.MetricMappings_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.MetricMappings_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])
	);


GO

CREATE NONCLUSTERED INDEX [IX_BusinessSegmentID]
    ON [ndms].[MetricMappings]([BusinessSegmentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DivisionID]
    ON [ndms].[MetricMappings]([DivisionID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FacilityID]
    ON [ndms].[MetricMappings]([FacilityID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProductLineID]
    ON [ndms].[MetricMappings]([ProductLineID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DepartmentID]
    ON [ndms].[MetricMappings]([DepartmentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessID]
    ON [ndms].[MetricMappings]([ProcessID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_KPIID]
    ON [ndms].[MetricMappings]([KPIID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_MetricID]
    ON [ndms].[MetricMappings]([MetricID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[MetricMappings]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[MetricMappings]([LastModifiedBy] ASC);

