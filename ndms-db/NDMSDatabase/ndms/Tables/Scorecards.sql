CREATE TABLE [ndms].[Scorecards] (
    [ID]                       INT            IDENTITY (1, 1) NOT NULL, 
    [Name]                     NVARCHAR (100) NOT NULL,
    [ParentScorecardID]        INT            NULL,
    [BusinessSegmentID]        INT            NULL,
    [DivisionID]               INT            NULL,
	[FacilityID]               INT            NULL,
    [ProductLineID]            INT            NULL,
    [DepartmentID]             INT            NULL,
    [ProcessID]                INT            NULL,
    [IsBowlingChartApplicable] BIT            NOT NULL,
    [DrilldownLevel]		   INT            NOT NULL,
	[SortOrder]				   INT			  NOT NULL DEFAULT (1),
    [CreatedOn]                DATETIME       NOT NULL DEFAULT (getutcdate()),
    [CreatedBy]				   INT            NULL,
	[LastModifiedOn]           DATETIME       NOT NULL DEFAULT (getutcdate()),
    [LastModifiedBy]           INT            NULL,
    [IsActive]                 BIT            NOT NULL DEFAULT(1),
    CONSTRAINT [PK_ndms.Scorecards] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.Scorecards_ndms.Scorecards_ParentScorecardID] FOREIGN KEY ([ParentScorecardID]) REFERENCES [ndms].[Scorecards] ([ID]),    
	CONSTRAINT [FK_ndms.Scorecards_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.Scorecards_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])  
);
GO 

CREATE NONCLUSTERED INDEX [IX_ParentScorecardID]
    ON [ndms].[Scorecards]([ParentScorecardID] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[Scorecards]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[Scorecards]([LastModifiedBy] ASC);



