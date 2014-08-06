CREATE TABLE [dbo].[SPFormat] (
    [CigaretteLenght]        NVARCHAR(255)   NOT NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [FilterLenght]           NVARCHAR(255)   NOT NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_SPFormat_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

