using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
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
    internal static IPurchaseOrderTemplate CreateEmailMessage(int _itemId, SPListItem _item, EntitiesDataContext _EDC)
    {
      try
      {
        FreightPO _fpo = (from idx in _EDC.FreightPOLibrary
                          where idx.Identyfikator == _itemId
                          select idx).First();
        return new FreightPurchaseOrderTemplate()
        {
          EmaiAddressTo = String.IsNullOrEmpty(_fpo.EMail) ? CommonDefinition.UnknownEmail : _fpo.EMail,
          Encodedabsurl = new Uri((string)_item["EncodedAbsUrl"]),
          Modified = (DateTime)_item["Modified"],
          ModifiedBy = (string)_item["Editor"],
          DocumentName = _item.File.Name,
          FPO2CityTitle = _fpo.City.NotAvailable(),
          FPO2CommodityTitle = _fpo.Commodity.NotAvailable(),
          FPO2CountryTitle = _fpo.Country.NotAvailable(),
          FPO2RouteGoodsHandlingPO = _fpo.FreightPO0.NotAvailable(),
          FPO2TransportUnitTypeTitle = _fpo.TransportUnit,
          FPOLoadingDate = _fpo.LoadingDate.GetValueOrDefault(DateTime.MaxValue),
          FPO2WarehouseAddress = _fpo.WarehouseAddress.NotAvailable()
        };
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in CreateEmailMessage because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }

    #region IPurchaseOrderTemplate
    public string EmaiAddressTo{get; set;}
    public string PartnerTitle{get; set;}
    public string Subject{get; set;}
    #endregion
  }
}
