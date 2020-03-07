CREATE TABLE [ndms].[Processes] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (100) NOT NULL,
	[CreatedOn]      DATETIME       NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		 INT            NOT NULL DEFAULT (0),
	[LastModifiedOn] DATETIME       NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy] INT            NOT NULL DEFAULT (0),
    [IsActive] BIT            NOT NULL,
    CONSTRAINT [PK_ndms.Processes] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.Processes_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.Processes_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Name]
    ON ndms.Processes([Name] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON ndms.Processes([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON ndms.Processes([LastModifiedBy] ASC);