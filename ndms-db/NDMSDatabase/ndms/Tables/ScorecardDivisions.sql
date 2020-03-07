CREATE TABLE [ndms].[ScorecardDivisions]
(
	[ScorecardRefId] INT NOT NULL,
    [DivisionRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardDivisions] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [DivisionRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardDivisions_ndms.Divisions_DivisionRefId] FOREIGN KEY ([DivisionRefId]) REFERENCES [ndms].[Divisions] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardDivisions_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardDivisions]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardDivisions]([DivisionRefId] ASC);
GO