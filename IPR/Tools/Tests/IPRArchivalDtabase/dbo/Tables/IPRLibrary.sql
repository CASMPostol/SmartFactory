CREATE TABLE [dbo].[IPRLibrary] (
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [DocumentNo]             NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NULL,
    CONSTRAINT [PK_IPRLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

