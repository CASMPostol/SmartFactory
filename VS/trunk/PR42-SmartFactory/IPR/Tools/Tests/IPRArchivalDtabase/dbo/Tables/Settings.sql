CREATE TABLE [dbo].[Settings] (
    [Created]    DATETIME       NOT NULL,
    [CreatedBy]  NVARCHAR (255) NOT NULL,
    [ID]         INT            NOT NULL,
    [KeyValue]   NVARCHAR (255) NOT NULL,
    [Modified]   DATETIME       NOT NULL,
    [ModifiedBy] NVARCHAR (255) NOT NULL,
    [Title]      NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Settings_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

