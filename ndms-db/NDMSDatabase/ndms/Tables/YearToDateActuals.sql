--CREATE TABLE [ndms].[YearToDateActuals]
--(
--	[ID]	         INT			 NOT NULL,
--	[TargetID] INT			 NOT NULL,
--	[ActualValue]    DECIMAL (18, 2) NOT NULL, 
--	[CreatedOn]      DATETIME        NOT NULL,
--    [CreatedBy]		 INT             NOT NULL,
--	[LastModifiedOn] DATETIME        NOT NULL,
--    [LastModifiedBy] INT             NOT NULL,
--	CONSTRAINT [PK_ndms.YearToDateActuals] PRIMARY KEY CLUSTERED ([ID] ASC),
--	CONSTRAINT [FK_ndms.YearToDateActuals_ndms.Targets_TargetID] FOREIGN KEY ([TargetID]) REFERENCES [ndms].[Targets] ([ID]) ON DELETE CASCADE,
--	CONSTRAINT [FK_ndms.YearToDateActuals_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
--	CONSTRAINT [FK_ndms.YearToDateActuals_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
--);


--GO
--CREATE NONCLUSTERED INDEX [IX_TargetID]
--    ON [ndms].[YearToDateActuals]([TargetID] ASC);

--GO
--CREATE NONCLUSTERED INDEX [IX_CreatedBy]
--    ON [ndms].[YearToDateActuals]([CreatedBy] ASC);

--GO
--CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
--    ON [ndms].[YearToDateActuals]([LastModifiedBy] ASC);
