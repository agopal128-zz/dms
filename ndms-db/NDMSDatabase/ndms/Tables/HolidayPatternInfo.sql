CREATE TABLE [ndms].[HolidayPatternInfo]
(
	[ID] INT   IDENTITY (1, 1) NOT NULL,
	[HolidayPatternID]		INT	NOT NULL, 
	[Date]	  DATE	  NOT NULL,	
	[CreatedOn]           DATETIME        NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]      INT             NOT NULL,
	[IsActive]	BIT	NOT NULL DEFAULT (1),
	CONSTRAINT [PK_ndms.HolidayPatternInfo] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.HolidayPatternInfo_ndms.HolidayPattern_HolidayPatternID] FOREIGN KEY ([HolidayPatternID]) REFERENCES [ndms].[HolidayPatterns] ([ID]),
	CONSTRAINT [FK_ndms.HolidayPatternInfo_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.HolidayPatternInfo_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID]) 
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_HolidayPatternID_Date]
    ON [ndms].[HolidayPatternInfo]([HolidayPatternID] ASC, [Date] ASC);
