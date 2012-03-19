using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class FreightPurchaseOrderTemplate
  {
    public string FPO2RouteGoodsHandlingPO { get; set; }
    public string FPO2CountryTitle { get; set; }
    public string FPO2CityTitle { get; set; }
    public DateTime FPOLoadingDate { get; set; }
    public string FPO2TransportUnitTypeTitle { get; set; }
    public string FPO2CommodityTitle { get; set; }
    public DateTime Modified { get; set; }
    public string ModifiedBy { get; set; }
    public Uri Encodedabsurl { get; set; }
    public string DocumentName { get; set; }
    public string FPO2WarehouseAddress { get; set; }
  }
}
