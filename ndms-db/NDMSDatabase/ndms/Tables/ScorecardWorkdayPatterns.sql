CREATE TABLE [ndms].[ScorecardWorkdayPatterns]
(
	[ID] INT   IDENTITY (1, 1) NOT NULL,
	[ScorecardID]		INT	NOT NULL, 
	[IsSunday]	BIT	NOT NULL,
	[IsMonday]	BIT	NOT NULL ,
	[IsTuesday]	BIT	NOT NULL ,
	[IsWednesday]	BIT	NOT NULL ,
	[IsThursday]	BIT	NOT NULL,
	[IsFriday]	BIT	NOT NULL ,
	[IsSaturday]	BIT	NOT NULL ,
	[EffectiveStartDate]  DATE        NOT NULL,
	[EffectiveEndDate]    DATE         NULL,
	[CreatedOn]           DATETIME        NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]      INT             NOT NULL,
	CONSTRAINT [PK_ndms.ScorecardWorkdayPattern] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.ScorecardWorkdayPattern_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardWorkdayPattern_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardWorkdayPattern_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
)
