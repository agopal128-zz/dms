CREATE TABLE [ndms].[OrganizationalData]
(
	[ID] INT NOT NULL,
	[Name] VARCHAR (100) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT (1),
	CONSTRAINT [PK_ndms.OrganizationalData] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO 
