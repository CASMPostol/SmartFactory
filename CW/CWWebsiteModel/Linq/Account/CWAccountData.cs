//<summary>
//  Title   : Customs Warehouse Account Record Data
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs.Account;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data - registered as an external service
  /// </summary>
  public class CWAccountData: ICWAccountFactory
  {
    public CWAccountData() { }

    public void ProcessCustomsMessage( CommonAccountData commonData )
    {
      this.CommonAccountData = commonData;
      this.CWCertificate = "TBD";
      this.CWMassPerPackage = 0;
      this.CWPackageKg = 0;
      this.CWPackageUnits = 0;
      this.CWPzNo = "TBD";
      this.CWQuantity = 0;
      this.EntryDate = DateTime.Today.Date;
      this.Units = "TBT";
      //TODO how to assign values. More info required.
    }
    internal CommonAccountData CommonAccountData { get; private set; }
    internal string CWCertificate { get; private set; }  //TODO from Required documents. 
    internal double? CWMassPerPackage { get; private set; } //TODO Calculated
    internal double? CWPackageKg { get; private set; } // Good description
    internal double? CWPackageUnits { get; private set; } //Good description
    internal string CWPzNo { get; private set; } // Manualy
    internal double? CWQuantity { get; private set; } //Good descriptionc
    internal DateTime? EntryDate { get; private set; } // Today ?
    internal string Units { get; private set; } //TODO Good description - ??
    internal Clearence Clearence { get; private set; } //TODO 
    protected void AnalizeGoodsDescription( string goodsDescription )
    {
      CWQuantity = 0; //TODO
    }

    #region ICWAccountFactory Members
    /// <summary>
    /// Creates the Customs Warehousing account.
    /// </summary>
    /// <param name="accountData">The account data.</param>
    /// <param name="warning">The warnings collection.</param>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    void ICWAccountFactory.CreateCWAccount( CommonAccountData accountData, List<Customs.Warnning> warning, string requestUrl )
    {

      string _at = "Beginning";
      using ( Entities _edc = new Entities( requestUrl ) )
      {
        ProcessCustomsMessage( accountData );
        Clearence = Element.GetAtIndex<Clearence>( _edc.Clearence, accountData.ConsentLookup );
        if ( WebsiteModel.Linq.CustomsWarehouse.RecordExist( _edc, accountData.DocumentNo ) )
        {
          string _msg = "CW record with the same SAD document number: {0} exist";
          throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _msg, Clearence.DocumentNo ) );
        }
        CustomsWarehouse _cw = new CustomsWarehouse( _edc, this );
        _at = "new InsertOnSubmit";
        _edc.CustomsWarehouse.InsertOnSubmit( _cw );
        _edc.SubmitChanges();
        _cw.UpdateTitle();
        _edc.SubmitChanges();
      }
    }
    #endregion
  }
}
