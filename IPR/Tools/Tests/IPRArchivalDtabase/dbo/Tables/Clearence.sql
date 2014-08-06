CREATE TABLE [dbo].[Clearence] (
    [Archival]               BIT             NULL,
    [Clearence2SadGoodID]    INT             NULL,
    [ClearenceProcedure]     NVARCHAR(255)   NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [DocumentNo]             NVARCHAR(255)   NULL,
    [ID]                         INT            NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [ProcedureCode]          NVARCHAR(255)   NULL,
    [ReferenceNumber]        NVARCHAR(255)   NULL,
    [SADConsignmentLibraryIndex] INT             NULL,
    [SPStatus]               BIT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_Clearence_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Clearence_SADConsignment] FOREIGN KEY ([SADConsignmentLibraryIndex]) REFERENCES [dbo].[SADConsignment] ([ID]),
    CONSTRAINT [FK_Clearence_SADGood] FOREIGN KEY ([Clearence2SadGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

