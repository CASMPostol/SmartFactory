CREATE TABLE [dbo].[Consent] (
    [Author]              NVARCHAR (MAX) NULL,
    [ConsentDate]         DATETIME       NULL,
    [ConsentPeriod]       FLOAT (53)     NULL,
    [Created]             DATETIME       NULL,
    [Editor]              NVARCHAR (MAX) NULL,
    [ID]                  INT            NOT NULL,
    [IsIPR]               BIT            NULL,
    [Modified]            DATETIME       NULL,
    [ProductivityRateMax] FLOAT (53)     NULL,
    [ProductivityRateMin] FLOAT (53)     NULL,
    [Title]               NVARCHAR (MAX) NOT NULL,
    [ValidFromDate]       DATETIME       NULL,
    [ValidToDate]         DATETIME       NULL,
    [Version]             INT            NULL,
	[OnlySQL]			  BIT			 NOT NULL,
    CONSTRAINT [PK_Consent_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

