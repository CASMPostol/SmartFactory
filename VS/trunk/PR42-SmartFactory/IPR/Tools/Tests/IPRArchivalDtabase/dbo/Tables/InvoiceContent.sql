CREATE TABLE [dbo].[InvoiceContent] (
    [Archival]                  BIT            NULL,
    [Author]                    NVARCHAR (MAX) NULL,
    [Created]                   DATETIME       NULL,
    [Editor]                    NVARCHAR (MAX) NULL,
    [ID]                        INT            NOT NULL,
    [InvoiceContent2BatchIndex] INT            NULL,
    [InvoiceContentStatus]      NVARCHAR (MAX) NULL,
    [InvoiceIndex]              INT            NULL,
    [Modified]                  DATETIME       NULL,
    [ProductType]               NVARCHAR (MAX) NULL,
    [Quantity]                  FLOAT (53)     NULL,
    [SKUDescription]            NVARCHAR (MAX) NULL,
    [Title]                     NVARCHAR (MAX) NOT NULL,
    [Units]                     NVARCHAR (MAX) NULL,
    [Version]                   INT            NULL,
	[OnlySQL]					BIT			   NOT NULL,
	[UIVersionString]		    NVARCHAR(max)  NULL,
    CONSTRAINT [PK_InvoiceContent_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_InvoiceContent_Batch] FOREIGN KEY ([InvoiceContent2BatchIndex]) REFERENCES [dbo].[Batch] ([ID]),
    CONSTRAINT [FK_InvoiceContent_InvoiceLibrary] FOREIGN KEY ([InvoiceIndex]) REFERENCES [dbo].[InvoiceLibrary] ([ID])
);

