CREATE TABLE [ndms].[ProductLines] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
	[CreatedOn]      DATETIME       NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		 INT            NOT NULL DEFAULT (0),
	[LastModifiedOn] DATETIME       NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy] INT            NOT NULL DEFAULT (0),
	[IsActive]   BIT            NOT NULL,
    CONSTRAINT [PK_ndms.ProductLines] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.ProductLines_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.ProductLines_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON ndms.ProductLines([Name] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON ndms.ProductLines([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON ndms.ProductLines([LastModifiedBy] ASC);
