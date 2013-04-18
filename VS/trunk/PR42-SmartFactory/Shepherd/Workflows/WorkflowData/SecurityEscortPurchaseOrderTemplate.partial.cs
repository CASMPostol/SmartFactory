using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  internal interface IPurchaseOrderTemplate : IEmailGrnerator
  {
    string EmaiAddressTo { get; set; }
  }
  public partial class SecurityEscortPurchaseOrderTemplate : IPurchaseOrderTemplate
  {
    #region MyRegion
    public string SPOFreightPO { get; set; }
    public string FPO2WarehouseAddress { get; set; }
    public string SPO2CityTitle { get; set; }
    public string SPO2CountryTitle { get; set; }
    public DateTime SPODispatchDate { get; set; }
    public string SPO2CommodityTitle { get; set; }
    public Uri Encodedabsurl { get; set; }
    public string DocumentName { get; set; }
    public DateTime Modified { get; set; }
    public string ModifiedBy { get; set; }
    internal static IPurchaseOrderTemplate CreateEmailMessage(SPListItem _item, EntitiesDataContext _EDC)
    {
      try
      {
        EscortPO _fpo = (from idx in _EDC.EscortPOLibrary
                         where idx.Identyfikator == _item.ID
                         select idx).First();
        return new SecurityEscortPurchaseOrderTemplate()
        {
          SPOFreightPO = _fpo.SPOFreightPO.NotAvailable(),
          EmaiAddressTo = _fpo.EmailAddress.NotAvailable(),
          Encodedabsurl = new Uri((string)_item["EncodedAbsUrl"]),
          Modified = (DateTime)_item["Modified"],
          ModifiedBy = ((string)_item["Editor"]).NotAvailable(),
          DocumentName = _item.File.Name.NotAvailable(),
          SPO2CityTitle = _fpo.SecurityPOCity.NotAvailable(),
          SPO2CommodityTitle = _fpo.SecurityPOCommodity.NotAvailable(),
          SPO2CountryTitle = _fpo.SecurityPOCountry.NotAvailable(),
          SPODispatchDate = _fpo.SPODispatchDate.GetValueOrDefault(DateTime.MaxValue),
          FPO2WarehouseAddress = _fpo.FPOWarehouseAddress.NotAvailable(),
        };
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in CreateEmailMessage because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    #endregion

    #region IEmailGrnerator
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    public string EmaiAddressTo { get; set; }
    public DateTime StartTime { get; set; }
    public string ShippingTitle { get; set; }
    #endregion
  }
}
