CREATE TABLE [ndms].[ScorecardHolidayPatterns]
(
    [ID] INT   IDENTITY (1, 1) NOT NULL,
	[ScorecardID]		INT	NOT NULL,
	[HolidayPatternID]		INT	NOT NULL, 
	[EffectiveStartDate]           DATE        NOT NULL,
	[EffectiveEndDate]           DATE         NULL,
	[CreatedOn]           DATETIME        NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]      INT             NOT NULL,
	CONSTRAINT [PK_ndms.ScorecardHolidayPattern] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.ScorecardHolidayPattern_ndms.Scorecards_ScorecardID] FOREIGN KEY ([ScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardHolidayPattern_ndms.HolidayPattern_HolidayPatternID] FOREIGN KEY ([HolidayPatternID]) REFERENCES [ndms].[HolidayPatterns] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardHolidayPattern_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.ScorecardHolidayPattern_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
)
