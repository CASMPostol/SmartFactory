CREATE TABLE [dbo].[JSOXCustomsSummary] (
    [CompensationGood]             NVARCHAR (255) NOT NULL,
    [Created]                      DATETIME       NOT NULL,
    [CreatedBy]                    NVARCHAR (255) NOT NULL,
    [CustomsProcedure]             NVARCHAR (255) NOT NULL,
    [ExportOrFreeCirculationSAD]   NVARCHAR (255) NOT NULL,
    [ID]                           INT            NOT NULL,
    [IntroducingSADDate]           DATETIME       NOT NULL,
    [IntroducingSADNo]             NVARCHAR (255) NOT NULL,
    [InvoiceNo]                    NVARCHAR (255) NOT NULL,
    [JSOXCustomsSummary2JSOXIndex] INT            NOT NULL,
    [Modified]                     DATETIME       NOT NULL,
    [ModifiedBy]                   NVARCHAR (255) NOT NULL,
    [RemainingQuantity]            FLOAT (53)     NOT NULL,
    [SADDate]                      DATETIME       NOT NULL,
    [Title]                        NVARCHAR (255) NOT NULL,
    [TotalAmount]                  FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_JSOXCustomsSummary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_JSOXCustomsSummary_JSOXLibrary] FOREIGN KEY ([JSOXCustomsSummary2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

