CREATE TABLE [dbo].[ActivitiesLogs] (
    [ID]        INT            IDENTITY (1, 1) NOT NULL,
    [Operation] NVARCHAR (255) NOT NULL,
    [Date]      DATETIME       NOT NULL,
    [UserName]  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ActivitiesLogs_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

