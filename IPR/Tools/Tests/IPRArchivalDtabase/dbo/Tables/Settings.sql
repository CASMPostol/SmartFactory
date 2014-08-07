CREATE TABLE [dbo].[Settings] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [KeyValue]               NVARCHAR(255)   NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_Settings_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

