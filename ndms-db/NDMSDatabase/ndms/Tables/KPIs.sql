CREATE TABLE [ndms].[KPIs] (
    [ID]       INT           NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [IsActive] BIT           NOT NULL,
    CONSTRAINT [PK_ndms.KPIs] PRIMARY KEY CLUSTERED ([ID] ASC)
);

