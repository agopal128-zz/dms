CREATE TABLE [ndms].[DailyTargets] (
    [ID]              INT             IDENTITY (1, 1) NOT NULL,
    [MonthlyTargetID] INT             NOT NULL,
    [Day]             INT             NOT NULL,	
    [MaxGoalValue]    DECIMAL (18, 2) NULL,
	[RolledUpGoalValue]    DECIMAL (18, 2) NULL,
	[IsManual]		  BIT			  DEFAULT(0),
	[CreatedOn]		  DATETIME		  NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]		  INT			  NOT NULL,
	[LastModifiedOn]  DATETIME	      NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]  INT			  NOT NULL,
    CONSTRAINT [PK_ndms.DailyTargets] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.DailyTargets_ndms.MonthlyTargets_MonthlyTargetID] FOREIGN KEY ([MonthlyTargetID]) REFERENCES [ndms].[MonthlyTargets] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_ndms.DailyTargets_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.DailyTargets_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
);


GO
CREATE NONCLUSTERED INDEX [IX_MonthlyTargetID]
    ON [ndms].[DailyTargets]([MonthlyTargetID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[DailyTargets]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[DailyTargets]([LastModifiedBy] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_MonthlyTargetID_Day]
    ON [ndms].[DailyTargets]([MonthlyTargetID] ASC, [Day] ASC);

