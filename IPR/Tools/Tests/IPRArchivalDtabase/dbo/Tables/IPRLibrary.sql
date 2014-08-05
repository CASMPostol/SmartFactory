CREATE TABLE [dbo].[IPRLibrary] (
    [Created]    DATETIME       NOT NULL,
    [CreatedBy]  NVARCHAR (255) NOT NULL,
    [DocumentNo] NVARCHAR (255) NOT NULL,
    [ID]         INT            NOT NULL,
    [Modified]   DATETIME       NOT NULL,
    [ModifiedBy] NVARCHAR (255) NOT NULL,
    [Title]      NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_IPRLibrary_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

