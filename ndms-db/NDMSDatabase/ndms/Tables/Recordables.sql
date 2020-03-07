CREATE TABLE [ndms].[Recordables]
(
	[ID] INT IDENTITY(1, 1) NOT NULL,
	[ScorecardID] INT NOT NULL, 
    [RecordableDate] DATE NOT NULL, 
    [IsManual] BIT NOT NULL DEFAULT (0),     
    [CreatedOn] DATETIME NOT NULL DEFAULT (getutcdate()), 
    [CreatedBy] INT NULL,
	[LastModifiedOn]	DATETIME NOT NULL DEFAULT (getutcdate()),
	[LastModifiedBy]	INT      NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT (1), 
	CONSTRAINT [PK_ndms.Recordables] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.Recordables_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_ScorecardID_RecordableDate]
ON [ndms].[Recordables]([ScorecardID] ASC,[RecordableDate] ASC);

GO

CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[Recordables]([CreatedBy] ASC);

GO

CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[Recordables]([LastModifiedBy] ASC);
