CREATE TABLE [dbo].[CutfillerCoefficient] (
    [CFTProductivityNormMax] FLOAT           NULL,
    [CFTProductivityNormMin] FLOAT           NULL,
    [CFTProductivityRateMax] FLOAT           NULL,
    [CFTProductivityRateMin] FLOAT           NULL,
    [Created]                DATETIME        NULL,
    [CreatedBy]              NVARCHAR(255)   NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [ModifiedBy]             NVARCHAR(255)   NULL,
    CONSTRAINT [PK_CutfillerCoefficient_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

