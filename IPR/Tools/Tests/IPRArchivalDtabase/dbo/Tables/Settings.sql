CREATE TABLE [dbo].[Settings] (
    [Author]   NVARCHAR (MAX) NULL,
    [Created]  DATETIME       NULL,
    [Editor]   NVARCHAR (MAX) NULL,
    [ID]       INT            NOT NULL,
    [KeyValue] NVARCHAR (MAX) NOT NULL,
    [Modified] DATETIME       NULL,
    [Title]    NVARCHAR (MAX) NOT NULL,
    [Version]  INT            NULL,
    CONSTRAINT [PK_Settings_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

