CREATE TABLE [ndms].[Holidays]
(
	[ID]	 INT IDENTITY (1, 1) NOT NULL, 
    [Name]	 NVARCHAR(100)		 NOT NULL, 
    [Date]	 DATE				 NOT NULL,
	[YearID] INT                 NOT NULL, 
    CONSTRAINT [PK_ndms.Holidays] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.Holidays_ndms.Years_YearID] FOREIGN KEY ([YearID]) REFERENCES [ndms].[Years] ([ID])
);

GO

CREATE NONCLUSTERED INDEX [IX_YearID]
    ON [ndms].[Holidays]([YearID] ASC);
