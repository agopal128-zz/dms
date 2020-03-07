CREATE TABLE [ndmshistory].[MonthlyActualsHistory]
(
	[ID]	             INT			 IDENTITY (1, 1) NOT NULL,
	[MonthlyActualID]    INT			 NOT NULL,
	[TargetID]			 INT			 NOT NULL,
	[Month]	             INT			 NOT NULL,
	[ActualValue]	     DECIMAL (18, 2) NULL,
	[Status]             INT             NOT NULL,
	[CreatedOn]          DATETIME        NOT NULL,
    [CreatedBy]		     INT             NOT NULL,
	[LastModifiedOn]     DATETIME        NOT NULL,
    [LastModifiedBy]     INT             NOT NULL,
	CONSTRAINT [PK_ndms.MonthlyActualsHistory] PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_ndms.MonthlyActualsHistory_ndms.MonthlyActuals_MonthlyActualID] FOREIGN KEY ([MonthlyActualID]) REFERENCES [ndms].[MonthlyActuals] ([ID]),
	CONSTRAINT [FK_ndms.MonthlyActualsHistory_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.MonthlyActualsHistory_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.MonthlyActualsHistory_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])
);
GO
CREATE NONCLUSTERED INDEX [IX_TargetID]
    ON [ndmshistory].[MonthlyActualsHistory]([TargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_MonthlyActualID]
    ON [ndmshistory].[MonthlyActualsHistory]([MonthlyActualID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndmshistory].[MonthlyActualsHistory]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndmshistory].[MonthlyActualsHistory]([LastModifiedBy] ASC);