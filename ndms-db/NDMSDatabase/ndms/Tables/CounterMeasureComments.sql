CREATE TABLE [ndms].[CounterMeasureComments]
(
	[ID]			   INT			    IDENTITY (1, 1) NOT NULL, 
    [CounterMeasureID] INT			    NOT NULL, 
    [Comment]		   NVARCHAR(4000)	NOT NULL, 
    [CreatedOn]		   DATETIME		    NOT NULL, 
    [CreatedBy]		   INT              NOT NULL, 
    [LastModifiedOn]   DATETIME         NOT NULL, 
    [LastModifiedBy]   INT              NOT NULL,
	CONSTRAINT [PK_ndms.CounterMeasureComments] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_ndms.CounterMeasureComments_ndms.CounterMeasures_CounterMeasureID] FOREIGN KEY ([CounterMeasureID]) REFERENCES [ndms].[CounterMeasures] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasureComments_ndms.Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [ndms].[Users] ([ID]),
	CONSTRAINT [FK_ndms.CounterMeasureComments_ndms.Users_LastModifiedBy] FOREIGN KEY ([LastModifiedBy]) REFERENCES [ndms].[Users] ([ID])   
);

GO
CREATE NONCLUSTERED INDEX [IX_CounterMeasureID]
    ON [ndms].[CounterMeasureComments]([CounterMeasureID] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_CreatedBy]
    ON [ndms].[CounterMeasureComments]([CreatedBy] ASC);

GO
CREATE NONCLUSTERED INDEX [IX_LastModifiedBy]
    ON [ndms].[CounterMeasureComments]([LastModifiedBy] ASC);