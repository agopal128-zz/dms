CREATE TABLE [ndms].[HolidayPatterns]
(
	[ID] INT   IDENTITY (1, 1) NOT NULL,
	[Name]		NVARCHAR(100)	NOT NULL, 	
	[CreatedOn]           DATETIME        NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		      INT             NOT NULL,
	[LastModifiedOn]      DATETIME        NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]      INT             NOT NULL,
	[IsActive]	BIT				NOT NULL DEFAULT (1),
	CONSTRAINT [PK_ndms.HolidayPattern] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.HolidayPattern_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.HolidayPattern_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
)
