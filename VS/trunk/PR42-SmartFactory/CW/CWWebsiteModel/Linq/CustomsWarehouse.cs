//<summary>
//  Title   : Customs Warehousing Account Class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Customs Warehousing Account Class
  /// </summary>
  public partial class CustomsWarehouse
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="CW" /> class.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="declaration">The declaration.</param>
    public CustomsWarehouse(Linq.Entities edc, Account.CWAccountData data)
      : this()
    {
      this.AccountBalance = data.CWQuantity;
      this.Batch = data.CommonAccountData.BatchId;
      this.ClosingDate = Extensions.DateTimeNull;
      this.Currency = "PLN";
      this.CustomsDebtDate = data.CommonAccountData.CustomsDebtDate;
      this.CWL_CW2ClearenceID = data.ClearenceLookup;
      this.CWL_CW2ConsentTitle = data.ConsentLookup;
      this.CWL_CW2PCNID = data.PCNTariffCodeLookup;
      this.CWL_CW2CWLibraryIDId = null;
      this.CW_MassPerPackage = data.CWMassPerPackage;
      this.CW_PackageKg = data.CWPackageKg;
      this.CW_PackageUnits = data.CWPackageUnits;
      //this.CW_PzNo = "N/A";
      this.CW_Quantity = data.CWQuantity;
      this.DocumentNo = data.CommonAccountData.DocumentNo;
      this.CWC_EntryDate = data.EntryDate;
      this.Grade = data.CommonAccountData.GradeName;
      this.GrossMass = data.CommonAccountData.GrossMass;
      this.InvoiceNo = data.CommonAccountData.Invoice;
      this.NetMass = data.CommonAccountData.NetMass;
      this.SKU = data.CommonAccountData.SKU;
      this.TobaccoNotAllocated = data.CWQuantity;
      this.TobaccoName = data.CommonAccountData.TobaccoName;
      this.Title = "-- creating -- ";
      this.Units = data.Units;
      this.ValidToDate = data.ValidToDate;

      //Certificate
      this.CW_CertificateOfOrgin = data.CW_CertificateOfOrgin;
      this.CW_CertificateOfAuthenticity = data.CW_CertificateOfAuthenticity;
      if (data.CW_COADate.HasValue)
        this.CW_COADate = data.CW_COADate;
      if (data.CW_CODate.HasValue)
        this.CW_CODate = data.CW_CODate;
      this.CWL_CW2VendorTitle = data.VendorLookup;
    }
    public static void Dispose(Entities entities, string batch, DisposalRequestLib parentRequestLib, CustomsWarehouseDisposal.XmlData xmlData)
    {
      foreach (CustomsWarehouse _cwx in GetOrderedQueryable4Batch(entities, batch))
      {
        _cwx.Dispose(entities, parentRequestLib, ref xmlData);
        if (xmlData.DeclaredQuantity + xmlData.AdditionalQuantity <= 0)
          return;
      }
      string _msg = String.Format("there is enought tobacco {0} to dispose the batch: {1}", xmlData.DeclaredQuantity + xmlData.AdditionalQuantity, batch);
      throw new CAS.SharePoint.ApplicationError("CustomsWarehouse.Dispose", "ending", _msg, null);
    }
    internal static bool RecordExist(Entities entities, string documentNo)
    {
      return (from CustomsWarehouse _cwx in entities.CustomsWarehouse where _cwx.DocumentNo.Contains(documentNo) select _cwx).Any();
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle()
    {
      Title = String.Format("CW-{0:D4}{1:D6}", this.CWC_EntryDate.Value.Year, Id.Value);
    }
    #endregion

    #region private
    private static IOrderedQueryable<CustomsWarehouse> GetOrderedQueryable4Batch(Entities entities, string batch)
    {
      return from _cwi in entities.CustomsWarehouse
             where _cwi.TobaccoNotAllocated.Value >= 0
             orderby _cwi.Id.Value ascending
             select _cwi;
    }
    private void Dispose(Entities entities, DisposalRequestLib parent, ref CustomsWarehouseDisposal.XmlData _xmlData)
    {
      double _AddedKg = 0;
      double _DeclaredNetMass = 0;
      double _2Dispose = _xmlData.DeclaredQuantity + _xmlData.AdditionalQuantity;
      double _BoxWeight = this.CW_PackageUnits.Value == 0 ? 0 : (this.GrossMass.Value - this.CW_Quantity.Value) / this.CW_PackageUnits.Value;
      if (this.TobaccoNotAllocated < _2Dispose)
        _2Dispose = this.TobaccoNotAllocated.Value;
      double _boxes = Math.Round(_2Dispose / this.CW_MassPerPackage.Value + 0.5, 0);
      _2Dispose = _boxes * this.CW_MassPerPackage.Value;
      Debug.Assert(_2Dispose <= this.TobaccoNotAllocated.Value, "Account overrun");
      if (Math.Abs(_2Dispose - this.TobaccoNotAllocated.Value) < this.CW_MassPerPackage.Value)
        _2Dispose = this.TobaccoNotAllocated.Value;
      _DeclaredNetMass = Math.Min(_2Dispose, _xmlData.DeclaredQuantity);
      _AddedKg = Math.Max(_2Dispose - _DeclaredNetMass, 0);
      _AddedKg = Math.Min(_AddedKg, _xmlData.AdditionalQuantity);
      _xmlData.DeclaredQuantity -= _DeclaredNetMass;
      _xmlData.AdditionalQuantity -= _AddedKg;
      CustomsWarehouseDisposal _new = new CustomsWarehouseDisposal()
      {
        AccountClosed = false,
        Archival = false,
        Batch = this.Batch,//TODO secondary
        Currency = this.Currency,// TODO secondary
        CustomsStatus = CustomsStatus.NotStarted,
        CW_AddedKg = _AddedKg,
        CW_DeclaredNetMass = _DeclaredNetMass,
        CW_SettledNetMass = _2Dispose,
        CW_SettledGrossMass = _2Dispose + _BoxWeight * _boxes,
        CW_PackageToClear = _boxes,
        CWL_CWDisposal2DisposalRequestLibraryID = parent,
        CWL_CWDisposal2PCNTID = this.CWL_CW2PCNID,
        Grade = this.Grade,//TODO secondary
        SKU = this.SKU,//TODO secondary
        SKUDescription = _xmlData.SKUDescription,
        Title = "ToDo",
        TobaccoName = this.TobaccoName,//TODO secondary lookup 
      };
      _new.UpdateTitle(this.CWC_EntryDate.Value);
      entities.CustomsWarehouseDisposal.InsertOnSubmit(_new);
    }
    #endregion
  }
}
