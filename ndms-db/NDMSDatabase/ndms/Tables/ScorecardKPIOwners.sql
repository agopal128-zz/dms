CREATE TABLE [ndms].[ScorecardKPIOwners] (
    [ID]            INT            IDENTITY (1, 1) NOT NULL,
    [ScorecardID]   INT            NOT NULL,
    [UserID]		INT			   NOT NULL,
    [AssignedOn]    DATETIME       NOT NULL DEFAULT (getutcdate()),
    [DeactivatedOn] DATETIME       NULL,
    [IsActive]      BIT            NOT NULL DEFAULT(1),
    CONSTRAINT [PK_ndms.ScorecardKPIOwners] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.ScorecardKPIOwners_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.ScorecardKPIOwners_ndms.Users_UserID] FOREIGN KEY ([UserID]) REFERENCES [ndms].[Users] ([ID]) ON DELETE CASCADE,
);


GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Scorecard_KPIOwner]
    ON [ndms].[ScorecardKPIOwners]([ScorecardID] ASC, [UserID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_ScorecardID]
    ON [ndms].[ScorecardKPIOwners]([ScorecardID] ASC);
		
GO
CREATE NONCLUSTERED INDEX [IX_UserID]
    ON [ndms].[ScorecardKPIOwners]([UserID] ASC);

