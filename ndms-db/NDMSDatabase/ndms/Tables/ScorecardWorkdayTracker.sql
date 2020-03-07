CREATE TABLE [ndms].[ScorecardWorkdayTracker]
(
	[ID] INT   IDENTITY (1, 1) NOT NULL,
	[ScorecardID]		INT	NOT NULL, 
	[Date]	  DATE	  NOT NULL,
	[IsWorkDay]	BIT	NOT NULL,	
	[CreatedOn]           DATETIME        NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]      INT             NOT NULL,
	[IsActive]	BIT	NOT NULL DEFAULT (1),
	CONSTRAINT [PK_ndms.ScorecardWorkdayTracker] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.ScorecardWorkdayTracker_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardWorkdayTracker_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardWorkdayTracker_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
)
