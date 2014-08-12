CREATE TABLE [dbo].[SADDuties] (
    [Amount]              FLOAT (53)     NULL,
    [Archival]            BIT            NULL,
    [Author]              NVARCHAR (MAX) NULL,
    [Created]             DATETIME       NULL,
    [DutyType]            NVARCHAR (MAX) NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [ID]                  INT            NOT NULL,
    [Modified]            DATETIME       NULL,
    [SADDuties2SADGoodID] INT            NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [Version]             INT            NULL,
    CONSTRAINT [PK_SADDuties_ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_SADDuties_SADGood] FOREIGN KEY ([SADDuties2SADGoodID]) REFERENCES [dbo].[SADGood] ([ID])
);

