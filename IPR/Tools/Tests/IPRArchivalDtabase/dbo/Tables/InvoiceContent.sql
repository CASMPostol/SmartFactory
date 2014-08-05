CREATE TABLE [dbo].[InvoiceContent] (
    [Created]                   DATETIME       NOT NULL,
    [CreatedBy]                 NVARCHAR (255) NOT NULL,
    [ID]                        INT            NOT NULL,
    [InvoiceContent2BatchIndex] INT            NOT NULL,
    [InvoiceContentStatus]      NVARCHAR (255) NOT NULL,
    [InvoiceIndex]              INT            NOT NULL,
    [Modified]                  DATETIME       NOT NULL,
    [ModifiedBy]                NVARCHAR (255) NOT NULL,
    [ProductType]               NVARCHAR (255) NOT NULL,
    [Quantity]                  FLOAT (53)     NOT NULL,
    [SKUDescription]            NVARCHAR (255) NOT NULL,
    [Title]                     NVARCHAR (255) NOT NULL,
    [Units]                     NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_InvoiceContent_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceContent_Batch] FOREIGN KEY ([InvoiceContent2BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_InvoiceContent_InvoiceLibrary] FOREIGN KEY ([InvoiceIndex]) REFERENCES [dbo].[InvoiceLibrary] ([ID])
);

