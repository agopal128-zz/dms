CREATE TABLE [ndms].[Years]
(
	[ID]        INT			NOT NULL,
	[Name]		NVARCHAR(50) NOT NULL,
    [StartDate] DATE		NOT NULL, 
    [EndDate]   DATE		NOT NULL,
	CONSTRAINT [PK_ndms.Years] PRIMARY KEY CLUSTERED ([ID] ASC)
)
