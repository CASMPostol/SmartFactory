CREATE TABLE [dbo].[InvoiceLibrary] (
    [BillDoc]                NVARCHAR (255) NOT NULL,
    [ClearenceIndex]         INT            NOT NULL,
    [Created]                DATETIME       NOT NULL,
    [CreatedBy]              NVARCHAR (255) NOT NULL,
    [ID]                     INT            NOT NULL,
    [InvoiceCreationDate]    DATETIME       NOT NULL,
    [InvoiceLibraryReadOnly] BIT            NOT NULL,
    [InvoiceLibraryStatus]   BIT            NOT NULL,
    [IsExport]               BIT            NOT NULL,
    [Modified]               DATETIME       NOT NULL,
    [ModifiedBy]             NVARCHAR (255) NOT NULL,
    [Title]                  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_InvoiceLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceLibrary_Clearence] FOREIGN KEY ([ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID])
);

