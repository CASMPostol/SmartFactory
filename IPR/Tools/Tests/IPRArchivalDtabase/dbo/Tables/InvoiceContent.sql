CREATE TABLE [dbo].[InvoiceContent] (
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [InvoiceContent2BatchIndex] INT             NULL,
    [InvoiceContentStatus]   NVARCHAR(255)   NULL,
    [InvoiceIndex]           INT             NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [ProductType]            NVARCHAR(255)   NULL,
    [Quantity]               FLOAT           NULL,
    [SKUDescription]         NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    [Units]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_InvoiceContent_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceContent_Batch] FOREIGN KEY ([InvoiceContent2BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_InvoiceContent_InvoiceLibrary] FOREIGN KEY ([InvoiceIndex]) REFERENCES [dbo].[InvoiceLibrary] ([ID])
);

