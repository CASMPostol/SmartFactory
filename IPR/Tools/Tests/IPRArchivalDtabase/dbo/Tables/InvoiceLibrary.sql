CREATE TABLE [dbo].[InvoiceLibrary] (
    [Author]                 NVARCHAR (MAX) NULL,
    [BillDoc]                NVARCHAR (MAX) NULL,
    [ClearenceIndex]         INT            NULL,
    [Created]                DATETIME       NULL,
    [DocumentCreatedBy]      NVARCHAR (MAX) NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [FileName]               NVARCHAR (MAX) NOT NULL,
    [ID]                     INT            NOT NULL,
    [InvoiceCreationDate]    DATETIME       NULL,
    [InvoiceLibraryReadOnly] BIT            NULL,
    [InvoiceLibraryStatus]   BIT            NULL,
    [IsExport]               BIT            NULL,
    [Modified]               DATETIME       NULL,
    [DocumentModifiedBy]     NVARCHAR (MAX) NULL,
    [Title]                  NVARCHAR (MAX) NULL,
    [Version]                INT            NULL,
	[OnlySQL]				 BIT			NOT NULL,
    CONSTRAINT [PK_InvoiceLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceLibrary_Clearence] FOREIGN KEY ([ClearenceIndex]) REFERENCES [dbo].[Clearence] ([ID])
);

