CREATE TABLE [dbo].[CutfillerCoefficient] (
    [CFTProductivityNormMax] NVARCHAR (255) NOT NULL,
    [CFTProductivityNormMin] NVARCHAR (255) NOT NULL,
    [CFTProductivityRateMax] FLOAT (53)     NOT NULL,
    [CFTProductivityRateMin] FLOAT (53)     NOT NULL,
    [Created]                DATETIME       NOT NULL,
    [CreatedBy]              NVARCHAR (255) NOT NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NOT NULL,
    [ModifiedBy]             NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_CutfillerCoefficient_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

