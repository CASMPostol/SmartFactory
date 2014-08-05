CREATE TABLE [dbo].[History] (
    [ID]         INT            IDENTITY (1, 1) NOT NULL,
    [ListName]   NVARCHAR (255) NOT NULL,
    [ItemID]     INT            NOT NULL,
    [FieldName]  NVARCHAR (255) NOT NULL,
    [FieldValue] NVARCHAR (MAX) NOT NULL,
    [Modified]   DATETIME       NOT NULL,
    [ModifiedBy] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_History_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

