CREATE TABLE [ndms].[Users] (
    [ID]             INT            IDENTITY (1, 1) NOT NULL,
    [AccountName]    NVARCHAR (100) NOT NULL,
    [FirstName]      NVARCHAR (200) NULL,
    [LastName]       NVARCHAR (200) NULL,
    [Email]          NVARCHAR (200) NULL,
    [LastLocationID] INT            NULL,
    [IsAdmin]        BIT            NOT NULL DEFAULT (0),
    [IsActive]       BIT            NOT NULL DEFAULT (1),
    [DateCreated]    DATETIME       NOT NULL DEFAULT (getutcdate()),
    [DateModified]   DATETIME       NOT NULL DEFAULT (getutcdate()),
	CONSTRAINT [PK_ndms.Users] PRIMARY KEY CLUSTERED ([ID] ASC)
);

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AccountName]
    ON [ndms].[Users]([AccountName] ASC);
GO