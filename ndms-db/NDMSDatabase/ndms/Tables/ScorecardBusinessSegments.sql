CREATE TABLE [ndms].[ScorecardBusinessSegments]
(
	[ScorecardRefId] INT NOT NULL,
    [BussinessSegmentRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardBusinessSegments] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [BussinessSegmentRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardBusinessSegments_ndms.BusinessSegments_BussinessSegmentRefId] FOREIGN KEY ([BussinessSegmentRefId]) REFERENCES [ndms].[BusinessSegments] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardBusinessSegments_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardBusinessSegments]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardBusinessSegments]([BussinessSegmentRefId] ASC);
GO