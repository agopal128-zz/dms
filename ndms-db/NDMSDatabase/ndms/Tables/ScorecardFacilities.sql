CREATE TABLE [ndms].[ScorecardFacilities]
(
	[ScorecardRefId] INT NOT NULL,
    [FacilityRefId]  INT NOT NULL,
    CONSTRAINT [PK_ndms.ScorecardFacilities] PRIMARY KEY CLUSTERED ([ScorecardRefId] ASC, [FacilityRefId] ASC),
    CONSTRAINT [FK_ndms.ScorecardFacilities_ndms.Facilities_FacilityRefId] FOREIGN KEY ([FacilityRefId]) REFERENCES [ndms].[Facilities] ([ID]) ON DELETE CASCADE,
    CONSTRAINT [FK_ndms.ScorecardFacilities_ndms.Scorecards_ScorecardRefId] FOREIGN KEY ([ScorecardRefId]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_ScorecardRefId]
    ON [ndms].[ScorecardFacilities]([ScorecardRefId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BussinessSegmentRefId]
    ON [ndms].[ScorecardFacilities]([FacilityRefId] ASC);
GO
