CREATE TABLE [dbo].[JSOXCustomsSummary] (
    [Author]                       NVARCHAR (MAX) NULL,
    [CompensationGood]             NVARCHAR (MAX) NULL,
    [Created]                      DATETIME       NULL,
    [CustomsProcedure]             NVARCHAR (MAX) NULL,
    [Editor]                       NVARCHAR (MAX) NULL,
    [ExportOrFreeCirculationSAD]   NVARCHAR (MAX) NULL,
    [ID]                           INT            NOT NULL,
    [IntroducingSADDate]           DATETIME       NULL,
    [IntroducingSADNo]             NVARCHAR (MAX) NULL,
    [InvoiceNo]                    NVARCHAR (MAX) NULL,
    [JSOXCustomsSummary2JSOXIndex] INT            NULL,
    [Modified]                     DATETIME       NULL,
    [RemainingQuantity]            FLOAT (53)     NULL,
    [SADDate]                      DATETIME       NULL,
    [Title]                        NVARCHAR (MAX) NOT NULL,
    [TotalAmount]                  FLOAT (53)     NULL,
    [Version]                      INT            NULL,
    CONSTRAINT [PK_JSOXCustomsSummary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_JSOXCustomsSummary_JSOXLibrary] FOREIGN KEY ([JSOXCustomsSummary2JSOXIndex]) REFERENCES [dbo].[JSOXLibrary] ([ID])
);

