CREATE TABLE [dbo].[SHMenthol] (
    [Author]         NVARCHAR (MAX) NULL,
    [Created]        DATETIME       NULL,
    [Editor]         NVARCHAR (MAX) NULL,
    [ID]             INT            NOT NULL,
    [Modified]       DATETIME       NULL,
    [ProductType]    NVARCHAR (MAX) NOT NULL,
    [SHMentholRatio] FLOAT (53)     NOT NULL,
    [Version]        INT            NULL,
    CONSTRAINT [PK_SHMenthol_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

