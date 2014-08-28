CREATE TABLE [dbo].[ActivityLog] (
    [ActivityPriority]       NVARCHAR(max)   NULL,
    [ActivitySource]         NVARCHAR(max)   NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [Body]                   NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [Expires]                DATETIME        NULL,
    [ID]                     INT             NOT NULL,
    [Modified]               DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [Version]                INT             NULL,
    [OnlySQL]				 BIT			 NOT NULL,
	[UIVersionString]		 NVARCHAR(max)	 NULL,	
	CONSTRAINT [PK_ActivityLog_ID] PRIMARY KEY CLUSTERED ([ID] ASC) 
);

