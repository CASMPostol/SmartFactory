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
      this.Archival = false;
      this.AccountClosed = false;
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
    /// <summary>
    /// Disposes goods for selected batch.
    /// </summary>
    /// <param name="entities">The entities representing SharePoint lists.</param>
    /// <param name="batch">The batch.</param>
    /// <param name="parentRequestLib">The parent request library.</param>
    /// <param name="xmlData">The XML data to be disposed.</param>
    /// <exception cref="CAS.SharePoint.ApplicationError">CustomsWarehouse.Dispose;ending;null</exception>
    public static void Dispose(Entities entities, string batch, DisposalRequestLib parentRequestLib, CustomsWarehouseDisposal.XmlData xmlData)
    {
      foreach (CustomsWarehouse _cwx in GetOrderedQueryable4Batch(entities, batch))
      {
        _cwx.Dispose(entities, parentRequestLib, ref xmlData);
        if (xmlData.DeclaredQuantity + xmlData.AdditionalQuantity <= 0)
          return;
      }
      string _msg = String.Format("there is not enought tobacco {0} to dispose the batch: {1}", xmlData.DeclaredQuantity + xmlData.AdditionalQuantity, batch);
      throw new CAS.SharePoint.ApplicationError("CustomsWarehouse.Dispose", "ending", _msg, null);
    }
    /// <summary>
    /// Check if the any records exists.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="documentNo">The document no.</param>
    /// <returns></returns>
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
    private void Dispose(Entities entities, DisposalRequestLib parent, ref CustomsWarehouseDisposal.XmlData xmlData)
    {
      decimal _2Dispose = xmlData.DeclaredQuantity + xmlData.AdditionalQuantity;
      if (this.TobaccoNotAllocated.DecimalValue() < _2Dispose)
        _2Dispose = this.TobaccoNotAllocated.DecimalValue();
      decimal _Boxes = Math.Round(_2Dispose / Convert.ToDecimal(this.CW_MassPerPackage.Value + 0.5), 0);
      _2Dispose = _Boxes * this.CW_MassPerPackage.DecimalValue();
      Debug.Assert(_2Dispose <= this.TobaccoNotAllocated.DecimalValue(), "Account overrun");
      if (Math.Abs(_2Dispose - this.TobaccoNotAllocated.DecimalValue()) < this.CW_MassPerPackage.DecimalValue())
        _2Dispose = this.TobaccoNotAllocated.DecimalValue();
      //DeclaredQntty
      decimal _DeclaredQntty = Math.Min(_2Dispose, xmlData.DeclaredQuantity);
      xmlData.DeclaredQuantity -= _DeclaredQntty;
      //AdditionalQntty
      decimal _AdditionalQntty = 0;
      _AdditionalQntty = Math.Max(_2Dispose - _DeclaredQntty, 0);
      xmlData.AdditionalQuantity -= Math.Min(_AdditionalQntty, xmlData.AdditionalQuantity);
      this.TobaccoNotAllocated = Convert.ToDouble(this.TobaccoNotAllocated.DecimalValue() - _2Dispose);
      CustomsWarehouseDisposal _new = new CustomsWarehouseDisposal()
      {
        AccountClosed = false,
        Archival = false,
        CustomsStatus = CustomsStatus.NotStarted,
        CW_AddedKg = _AdditionalQntty.DoubleValue(),
        CW_DeclaredNetMass = _DeclaredQntty.DoubleValue(),
        CW_SettledNetMass = _2Dispose.DoubleValue(),
        CW_SettledGrossMass = (_2Dispose + PackageWeight() * _Boxes).DoubleValue(),
        CW_PackageToClear = _Boxes.DoubleValue(),
        CWL_CWDisposal2DisposalRequestLibraryID = parent,
        CWL_CWDisposal2PCNTID = this.CWL_CW2PCNID,
        CWL_CWDisposal2CustomsWarehouseID = this,
        SKUDescription = xmlData.SKUDescription,
        Title = "ToDo", 
      };
      _new.UpdateTitle(this.CWC_EntryDate.Value);
      entities.CustomsWarehouseDisposal.InsertOnSubmit(_new);
    }
    private decimal PackageWeight()
    {
      return this.CW_PackageUnits.Value == 0 ? 0m : (this.GrossMass.DecimalValue() - this.CW_Quantity.DecimalValue()) / this.CW_PackageUnits.DecimalValue();
    }
    #endregion

  }
}
