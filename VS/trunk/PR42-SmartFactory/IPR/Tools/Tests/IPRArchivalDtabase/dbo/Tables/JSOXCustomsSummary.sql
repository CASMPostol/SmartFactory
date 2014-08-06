CREATE TABLE [dbo].[JSOXCustomsSummary] (
    [CompensationGood]       NVARCHAR(255)   NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [CustomsProcedure]       NVARCHAR(255)   NULL,
    [ExportOrFreeCirculationSAD] NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [IntroducingSADDate]     DATETIME        NULL,
    [IntroducingSADNo]       NVARCHAR(255)   NULL,
    [InvoiceNo]              NVARCHAR(255)   NULL,
    [JSOXCustomsSummary2JSOXIndex] INT             NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [RemainingQuantity]      FLOAT           NULL,
    [SADDate]                DATETIME        NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [TotalAmount]            FLOAT           NULL,
    CONSTRAINT [PK_JSOXCustomsSummary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_JSOXCustomsSummary_JSOXLibrary] FOREIGN KEY ([JSOXCustomsSummary2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

