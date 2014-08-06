CREATE TABLE [dbo].[SADConsignment] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]         INT            NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_SADConsignment_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

