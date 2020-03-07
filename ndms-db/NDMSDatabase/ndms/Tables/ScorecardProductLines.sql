CREATE TABLE [ndms].[ScorecardProductLines]
(
	[ScorecardRefId] INT NOT NULL,
    [ProductLineRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardProductLines] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [ProductLineRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardProductLines_ndms.ProductLines_ProductLineRefId] FOREIGN KEY ([ProductLineRefId]) REFERENCES [ndms].[ProductLines] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardProductLines_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardProductLines]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardProductLines]([ProductLineRefId] ASC);
GO
