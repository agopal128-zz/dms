CREATE TABLE [ndms].[ScorecardProcesses]
(
	[ScorecardRefId] INT NOT NULL,
    [ProcessRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardProcesses] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [ProcessRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardProcesses_ndms.Processes_ProcessRefId] FOREIGN KEY ([ProcessRefId]) REFERENCES [ndms].[Processes] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardProcesses_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardProcesses]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardProcesses]([ProcessRefId] ASC);
GO
