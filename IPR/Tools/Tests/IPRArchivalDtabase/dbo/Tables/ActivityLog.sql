CREATE TABLE [dbo].[ActivityLog] (
    [ActivityPriority] NVARCHAR (MAX) NULL,
    [ActivitySource]   NVARCHAR (MAX) NULL,
    [Author]           NVARCHAR (MAX) NULL,
    [Body]             NVARCHAR (MAX) NULL,
    [Created]          DATETIME       NULL,
    [Editor]           NVARCHAR (MAX) NULL,
    [Expires]          DATETIME       NULL,
    [ID]               INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [Title]            NVARCHAR (MAX) NOT NULL,
    [Version]          INT            NULL,
    CONSTRAINT [PK_ActivityLog_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

