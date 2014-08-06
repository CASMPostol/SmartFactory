CREATE TABLE [dbo].[InvoiceLibrary] (
    [BillDoc]                NVARCHAR(255)   NULL,
    [ClearenceIndex]         INT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [InvoiceCreationDate]    DATETIME        NULL,
    [InvoiceLibraryReadOnly] BIT             NULL,
    [InvoiceLibraryStatus]   BIT             NULL,
    [IsExport]               BIT             NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_InvoiceLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceLibrary_Clearence] FOREIGN KEY ([ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID])
);

