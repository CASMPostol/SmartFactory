CREATE TABLE [dbo].[SADConsignment] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]         INT            NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_SADConsignment_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

