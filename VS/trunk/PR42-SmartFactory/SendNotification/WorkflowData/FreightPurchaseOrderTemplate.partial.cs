using System;
using System.Linq;
using CAS.SmartFactory.Shepherd.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class FreightPurchaseOrderTemplate : IPurchaseOrderTemplate
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
    internal static IPurchaseOrderTemplate CreateEmailMessage(SPListItem _item, EntitiesDataContext _EDC)
    {
      try
      {
        return new FreightPurchaseOrderTemplate()
        {
          EmaiAddressTo = String.IsNullOrEmpty((string)_item["EmailAddress"]) ? CommonDefinition.UnknownEmail : (string)_item["EmailAddress"],
          Encodedabsurl = new Uri((string)_item["EncodedAbsUrl"]),
          Modified = (DateTime)_item["Modified"],
          ModifiedBy = (string)_item["Editor"],
          DocumentName = _item.File.Name,
          FPO2CityTitle = ((string)_item["FreightPOCity"]).NotAvailable(),
          FPO2CommodityTitle = ((string)_item["FreightPOCommodity"]).NotAvailable(),
          FPO2CountryTitle = ((string)_item["FreightPOCountry"]).NotAvailable(),
          FPO2RouteGoodsHandlingPO = ((string)_item["FPOFreightPO"]).NotAvailable(),
          FPO2TransportUnitTypeTitle = (string)_item["FreightPOTransportUnitType"],
          FPOLoadingDate = ((Nullable<DateTime>)_item["FPOLoadingDate"]).GetValueOrDefault(DateTime.MaxValue),
          FPO2WarehouseAddress = _item["FPOWarehouseAddress"] == null ? String.Empty.NotAvailable() : _item["FPOWarehouseAddress"].ToString().NotAvailable()
        };
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in CreateEmailMessage because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }

    #region IPurchaseOrderTemplate
    public string EmaiAddressTo { get; set; }
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    public DateTime StartTime { get; set; }
    public string ShippingTitle { get; set; }
    #endregion
  }
}
