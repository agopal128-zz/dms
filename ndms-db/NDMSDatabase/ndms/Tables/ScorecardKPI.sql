CREATE TABLE [ndms].[ScorecardKPI] (
    [ScorecardRefId] INT NOT NULL,
    [KPIRefId]       INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardKPI] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [KPIRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardKPI_ndms.KPIs_KPIRefId] FOREIGN KEY ([KPIRefId]) REFERENCES [ndms].[KPIs] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardKPI_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardKPI]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_KPIRefId]
    ON [ndms].[ScorecardKPI]([KPIRefId] ASC);

