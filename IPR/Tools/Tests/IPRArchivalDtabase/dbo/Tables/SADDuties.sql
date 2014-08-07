CREATE TABLE [dbo].[SADDuties] (
    [Amount]                 FLOAT           NULL,
    [Archival]               BIT             NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [DutyType]               NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    [owshiddenversion]       INT             NULL,
    [SADDuties2SADGoodID]    INT             NULL,
    [Title]                  NVARCHAR(255)   NOT NULL,
    CONSTRAINT [PK_SADDuties_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDuties_SADGood] FOREIGN KEY ([SADDuties2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

