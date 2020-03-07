CREATE TABLE [ndms].[ScorecardDepartments]
(
	[ScorecardRefId] INT NOT NULL,
    [DepartmentRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardDepartments] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [DepartmentRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardDepartments_ndms.Departments_DepartmentRefId] FOREIGN KEY ([DepartmentRefId]) REFERENCES [ndms].[Departments] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardDepartments_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardDepartments]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardDepartments]([DepartmentRefId] ASC);
GO
