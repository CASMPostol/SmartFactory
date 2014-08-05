CREATE TABLE [dbo].[SADDuties] (
    [Amount]              FLOAT (53)     NOT NULL,
    [Created]             DATETIME       NOT NULL,
    [CreatedBy]           NVARCHAR (255) NOT NULL,
    [DutyType]            NVARCHAR (255) NOT NULL,
    [ID]                  INT            NOT NULL,
    [Modified]            DATETIME       NOT NULL,
    [ModifiedBy]          NVARCHAR (255) NOT NULL,
    [SADDuties2SADGoodID] INT            NOT NULL,
    [Title]               NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_SADDuties_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDuties_SADGood] FOREIGN KEY ([SADDuties2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

