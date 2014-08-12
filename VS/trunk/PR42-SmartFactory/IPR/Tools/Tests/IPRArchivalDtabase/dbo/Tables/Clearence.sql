CREATE TABLE [dbo].[Clearence] (
    [Archival]                   BIT            NULL,
    [Author]                     NVARCHAR (MAX) NULL,
    [Clearence2SadGoodID]        INT            NULL,
    [ClearenceProcedure]         NVARCHAR (MAX) NULL,
    [Created]                    DATETIME       NULL,
    [DocumentNo]                 NVARCHAR (MAX) NULL,
    [Editor]                     NVARCHAR (MAX) NULL,
    [ID]                         INT            NOT NULL,
    [Modified]                   DATETIME       NULL,
    [ProcedureCode]              NVARCHAR (MAX) NULL,
    [ReferenceNumber]            NVARCHAR (MAX) NULL,
    [SADConsignmentLibraryIndex] INT            NULL,
    [SPStatus]                   BIT            NULL,
    [Title]                      NVARCHAR (MAX) NOT NULL,
    [Version]                    INT            NULL,
    CONSTRAINT [PK_Clearence_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_Clearence_SADConsignment] FOREIGN KEY ([SADConsignmentLibraryIndex]) REFERENCES [dbo].[SADConsignment] ([ID]),
    CONSTRAINT [FK_Clearence_SADGood] FOREIGN KEY ([Clearence2SadGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

