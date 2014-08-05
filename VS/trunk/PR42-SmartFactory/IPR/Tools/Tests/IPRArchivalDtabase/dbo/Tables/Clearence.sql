CREATE TABLE [dbo].[Clearence] (
    [Clearence2SadGoodID]        INT            NOT NULL,
    [ClearenceProcedure]         NVARCHAR (255) NOT NULL,
    [Created]                    DATETIME       NOT NULL,
    [CreatedBy]                  NVARCHAR (255) NOT NULL,
    [DocumentNo]                 NVARCHAR (255) NOT NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NOT NULL,
    [ModifiedBy]                 NVARCHAR (255) NOT NULL,
    [ProcedureCode]              NVARCHAR (255) NOT NULL,
    [ReferenceNumber]            NVARCHAR (255) NOT NULL,
    [SADConsignmentLibraryIndex] INT            NOT NULL,
    [SPStatus]                   BIT            NOT NULL,
    [Title]                      NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Clearence_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Clearence_SADConsignment] FOREIGN KEY ([SADConsignmentLibraryIndex]) REFERENCES [dbo].[SADConsignment] ([ID]),
    CONSTRAINT [FK_Clearence_SADGood] FOREIGN KEY ([Clearence2SadGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

