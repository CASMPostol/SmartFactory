if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Shipping')
  drop table  Shipping;
CREATE TABLE [dbo].[Shipping] (
    [AdditionalCosts]        FLOAT           NULL,
    [ArrivalTime]            DATETIME        NULL,
    [Author]                 NVARCHAR(max)   NULL,
    [BusinessDescription]    NVARCHAR(max)   NULL,
    [CancelationReason]      NVARCHAR(max)   NULL,
    [ContainerNo]            NVARCHAR(max)   NULL,
    [Created]                DATETIME        NULL,
    [DockNumber]             NVARCHAR(max)   NULL,
    [Editor]                 NVARCHAR(max)   NULL,
    [EndTime]                DATETIME        NULL,
    [EstimateDeliveryTime]   DATETIME        NULL,
    [EuroPalletsQuantity]    FLOAT           NULL,
    [ID]                     INT             NOT NULL,
    [InduPalletsQuantity]    FLOAT           NULL,
    [IsOutbound]             BIT             NULL,
    [LoadingType]            NVARCHAR(max)   NULL,
    [Modified]               DATETIME        NULL,
    [owshiddenversion]       INT             NULL,
    [PartnerTitle]           INT             NULL,
    [PoLastModification]     DATETIME        NULL,
    [PoNumberMultiline]      NVARCHAR(max)   NULL,
    [ReportPeriod]           NVARCHAR(max)   NULL,
    [SecurityEscortCatalogTitle] INT             NULL,
    [SecuritySealProtocolIndex] INT             NULL,
    [Shipping2City]          INT             NULL,
    [Shipping2Currency4AddCosts] INT             NULL,
    [Shipping2Currency4CostsPerKU] INT             NULL,
    [Shipping2CurrencyForEscort] INT             NULL,
    [Shipping2CurrencyForFreight] INT             NULL,
    [Shipping2EscortPOIndex] INT             NULL,
    [Shipping2FreightPOIndex] INT             NULL,
    [Shipping2PartnerTitle]  INT             NULL,
    [Shipping2RouteTitle]    INT             NULL,
    [Shipping2TransportUnitType] INT             NULL,
    [Shipping2TruckTitle]    INT             NULL,
    [Shipping2WarehouseTitle] INT             NULL,
    [ShippingCarrierTitle]   NVARCHAR(max)   NULL,
    [ShippingCommodityTitle] NVARCHAR(max)   NULL,
    [ShippingCountryTitle]   NVARCHAR(max)   NULL,
    [ShippingDuration]       FLOAT           NULL,
    [ShippingFreightCost]    FLOAT           NULL,
    [ShippingFreightPayerTitle] NVARCHAR(max)   NULL,
    [ShippingRouteDepartureCity] NVARCHAR(max)   NULL,
    [ShippingSecurityCost]   FLOAT           NULL,
    [ShippingState]          NVARCHAR(max)   NULL,
    [StartTime]              DATETIME        NULL,
    [Title]                  NVARCHAR(max)   NOT NULL,
    [TotalCostsPerKU]        FLOAT           NULL,
    [TotalQuantityKU]        FLOAT           NULL,
    [TrailerCondition]       NVARCHAR(max)   NULL,
    [TrailerConditionComments] NVARCHAR(max)   NULL,
    [TrailerTitle]           INT             NULL,
    [TruckAwaiting]          BIT             NULL,
    [TruckTitle]             INT             NULL,
    [TSEndTime]              DATETIME        NULL,
    [TSStartTime]            DATETIME        NULL,
    [WarehouseEndTime]       DATETIME        NULL,
    [WarehouseStartTime]     DATETIME        NULL,
    [OnlySQL]				 BIT			 NOT NULL,	
	CONSTRAINT [PK_Shipping_ID] PRIMARY KEY CLUSTERED ([ID] ASC) ,
    CONSTRAINT [FK_Shipping_Partner] FOREIGN KEY ([PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_Shipping_SecurityEscortRoute] FOREIGN KEY ([SecurityEscortCatalogTitle]) REFERENCES [dbo].[SecurityEscortRoute] ([ID]),
    CONSTRAINT [FK_Shipping_SealProtocolLibrary] FOREIGN KEY ([SecuritySealProtocolIndex]) REFERENCES [dbo].[SealProtocolLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_City] FOREIGN KEY ([Shipping2City]) REFERENCES [dbo].[City] ([ID]),
    CONSTRAINT [FK_Shipping_Currency] FOREIGN KEY ([Shipping2Currency4AddCosts]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_Currency] FOREIGN KEY ([Shipping2Currency4CostsPerKU]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_Currency] FOREIGN KEY ([Shipping2CurrencyForEscort]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_Currency] FOREIGN KEY ([Shipping2CurrencyForFreight]) REFERENCES [dbo].[Currency] ([ID]),
    CONSTRAINT [FK_Shipping_EscortPOLibrary] FOREIGN KEY ([Shipping2EscortPOIndex]) REFERENCES [dbo].[EscortPOLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_FreightPOLibrary] FOREIGN KEY ([Shipping2FreightPOIndex]) REFERENCES [dbo].[FreightPOLibrary] ([ID]),
    CONSTRAINT [FK_Shipping_Partner] FOREIGN KEY ([Shipping2PartnerTitle]) REFERENCES [dbo].[Partner] ([ID]),
    CONSTRAINT [FK_Shipping_Route] FOREIGN KEY ([Shipping2RouteTitle]) REFERENCES [dbo].[Route] ([ID]),
    CONSTRAINT [FK_Shipping_TransportUnitType] FOREIGN KEY ([Shipping2TransportUnitType]) REFERENCES [dbo].[TransportUnitType] ([ID]),
    CONSTRAINT [FK_Shipping_Truck] FOREIGN KEY ([Shipping2TruckTitle]) REFERENCES [dbo].[Truck] ([ID]),
    CONSTRAINT [FK_Shipping_Warehouse] FOREIGN KEY ([Shipping2WarehouseTitle]) REFERENCES [dbo].[Warehouse] ([ID]),
    CONSTRAINT [FK_Shipping_Trailer] FOREIGN KEY ([TrailerTitle]) REFERENCES [dbo].[Trailer] ([ID]),
    CONSTRAINT [FK_Shipping_Truck] FOREIGN KEY ([TruckTitle]) REFERENCES [dbo].[Truck] ([ID]),
);