CREATE TABLE [dbo].[CutfillerCoefficient] (
    [Author]                 NVARCHAR (MAX) NULL,
    [CFTProductivityNormMax] FLOAT (53)     NULL,
    [CFTProductivityNormMin] FLOAT (53)     NULL,
    [CFTProductivityRateMax] FLOAT (53)     NULL,
    [CFTProductivityRateMin] FLOAT (53)     NULL,
    [Created]                DATETIME       NULL,
    [Editor]                 NVARCHAR (MAX) NULL,
    [ID]                     INT            NOT NULL,
    [Modified]               DATETIME       NULL,
    [Version]                INT            NULL,
    CONSTRAINT [PK_CutfillerCoefficient_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

