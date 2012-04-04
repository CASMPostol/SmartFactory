using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
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
    internal static IPurchaseOrderTemplate CreateEmailMessage(int _itemId, SPListItem _item, EntitiesDataContext _EDC)
    {
      try
      {
        EscortPO _fpo = (from idx in _EDC.EscortPOLibrary
                         where idx.Identyfikator == _itemId
                         select idx).First();
        return new SecurityEscortPurchaseOrderTemplate()
        {
          SPOFreightPO = _fpo.Title(),
          EmaiAddressTo = String.IsNullOrEmpty(_fpo.EMail) ? CommonDefinition.UnknownEmail : _fpo.EMail,
          Encodedabsurl = new Uri((string)_item["EncodedAbsUrl"]),
          Modified = (DateTime)_item["Modified"],
          ModifiedBy = (string)_item["Editor"],
          DocumentName = _item.File.Name,
          SPO2CityTitle = _fpo.City,
          SPO2CommodityTitle = _fpo.Commodity,
          SPO2CountryTitle = _fpo.Country,
          SPODispatchDate = _fpo.DispatchDate.GetValueOrDefault(DateTime.MaxValue),
          FPO2WarehouseAddress = _fpo.WarehouseAddress,
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
    public string PartnerTitle
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
    public string Subject
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
    public string EmaiAddressTo
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }
    #endregion
  }
}
