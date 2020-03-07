CREATE TABLE [ndms].[MonthlyActuals]
(
	[ID]	             INT			 IDENTITY (1, 1) NOT NULL,
	[TargetID]			 INT			 NOT NULL,
	[Month]	             INT			 NOT NULL,
	[ActualValue]	     DECIMAL (18, 2) NULL,
	[Status]             INT             NOT NULL,
	[CreatedOn]          DATETIME        NOT NULL,
    [CreatedBy]		     INT             NOT NULL,
	[LastModifiedOn]     DATETIME        NOT NULL,
    [LastModifiedBy]     INT             NOT NULL,
	CONSTRAINT [PK_ndms.MonthlyActuals] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.MonthlyActuals_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.MonthlyActuals_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.MonthlyActuals_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])
);
GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndms].[MonthlyActuals]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[MonthlyActuals]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[MonthlyActuals]([LastModifiedBy] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_TargetID_Month]
    ON [ndms].[MonthlyActuals]([TargetID] ASC, [Month] ASC);
