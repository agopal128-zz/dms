CREATE TABLE [ndms].[DataTypes]
(
	[ID]	   INT            NOT NULL,
	[Name]     NVARCHAR (100) NOT NULL,
	[IsActive] BIT            NOT NULL,
	CONSTRAINT [PK_ndms.[DataTypes] PRIMARY KEY CLUSTERED ([ID] ASC)
)
