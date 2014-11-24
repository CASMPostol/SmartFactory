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

using CAS.SharePoint;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
    /// <param name="edc">The <see cref="Linq.Entities"/> instance.</param>
    /// <param name="data">The data.</param>
    public CustomsWarehouse(Linq.Entities edc, Account.CWAccountData data)
      : this()
    {
      this.Archival = false;
      this.AccountClosed = false;
      this.AccountBalance = data.CWQuantity;
      this.Batch = data.CommonAccountData.BatchId;
      this.ClosingDate = Extensions.SPMinimum;
      this.Currency = "PLN";
      this.CustomsDebtDate = data.CommonAccountData.CustomsDebtDate;
      this.CWL_CW2ClearenceID = data.ClearenceLookup;
      this.CWL_CW2ConsentTitle = data.ConsentLookup;
      this.CWL_CW2PCNID = data.PCNTariffCodeLookup;
      this.CWL_CW2CWLibraryID = null;
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
      this.ValidToDate = data.ValidToDate.GetValueOrNull();

      //Certificate
      this.CW_CertificateOfOrgin = data.CW_CertificateOfOrgin;
      this.CW_CertificateOfAuthenticity = data.CW_CertificateOfAuthenticity;
      this.CW_COADate = data.CW_COADate.GetValueOrNull();
      this.CW_CODate = data.CW_CODate.GetValueOrNull();
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
      List<CustomsWarehouse> _available = GetOrderedQueryable4Batch(entities, batch);
      CustomsWarehouse _noValue = _available.FirstOrDefault<CustomsWarehouse>(x => !x.Value.HasValue);
      if (_noValue != null)
      {
        string _msg = String.Format("No Value assigned to the account {0} dept day: {1}", _noValue.DocumentNo, _noValue.CustomsDebtDate);
        throw new CAS.SharePoint.ApplicationError("CustomsWarehouse.Dispose", "No Value", _msg, null);
      }
      if (_available.Count == 0)
      {
        string _batchMsg = String.Format("I cannot find any account for the batch: {0}", batch);
        throw new CAS.SharePoint.ApplicationError("CustomsWarehouse.Dispose", "GetOrderedQueryable4Batch", _batchMsg, null);
      }
      foreach (CustomsWarehouse _cwx in _available)
      {
        _cwx.Dispose(entities, parentRequestLib, ref xmlData);
        if (xmlData.DeclaredQuantity + xmlData.AdditionalQuantity <= 0)
          return;
      }
      string _msg2 = String.Format("there is not enough tobacco {0} to dispose the batch: {1}", xmlData.DeclaredQuantity + xmlData.AdditionalQuantity, batch);
      throw new CAS.SharePoint.ApplicationError("CustomsWarehouse.Dispose", "ending", _msg2, null);
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
    internal double PackageWeight()
    {
      return this.CW_PackageUnits.Value == 0 ? 0 : (this.GrossMass.Value - this.CW_Quantity.Value) / this.CW_PackageUnits.Value;
    }
    /// <summary>
    /// Reverse lookup to <see cref="CustomsWarehouseDisposal" />.
    /// </summary>
    /// <param name="edc">The entities.</param>
    /// <param name="emptyListIfNew">if set to <c>true</c> return empty list.</param>
    /// <returns></returns>
    public IEnumerable<CustomsWarehouseDisposal> CustomsWarehouseDisposal(Entities edc, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new CustomsWarehouseDisposal[] { } : null;
      if (m_CustomsWarehouseDisposal == null)
        m_CustomsWarehouseDisposal = from _cwdx in edc.CustomsWarehouseDisposal let _id = _cwdx.CWL_CWDisposal2CustomsWarehouseID.Id.Value where _id == this.Id.Value select _cwdx;
      return m_CustomsWarehouseDisposal;
    }
    #endregion

    #region private

    private IEnumerable<CustomsWarehouseDisposal> m_CustomsWarehouseDisposal = null;
    private static List<CustomsWarehouse> GetOrderedQueryable4Batch(Entities entities, string batch)
    {
      return (from _cwi in entities.CustomsWarehouse
              where _cwi.TobaccoNotAllocated.Value > 0 && _cwi.Batch == batch
              orderby _cwi.CustomsDebtDate.Value ascending, _cwi.DocumentNo ascending
              select _cwi).ToList<CustomsWarehouse>();
    }
    private void Dispose(Entities entities, DisposalRequestLib parent, ref CustomsWarehouseDisposal.XmlData xmlData)
    {
      decimal _2Dispose = xmlData.DeclaredQuantity + xmlData.AdditionalQuantity;
      if (this.TobaccoNotAllocated.DecimalValue() < _2Dispose)
        _2Dispose = this.TobaccoNotAllocated.DecimalValue();
      decimal _Boxes = Math.Round(_2Dispose / Convert.ToDecimal(this.CW_MassPerPackage.Value) + 0.49m, 0);
      _2Dispose = _Boxes * this.CW_MassPerPackage.DecimalValue();
      Debug.Assert(_2Dispose <= this.TobaccoNotAllocated.DecimalValue(), "Account overrun");
      if (Math.Abs(_2Dispose - this.TobaccoNotAllocated.DecimalValue()) < this.CW_MassPerPackage.DecimalValue())
        _2Dispose = this.TobaccoNotAllocated.DecimalValue();
      //DeclaredQntty
      decimal _DeclaredQntty = Math.Min(_2Dispose, xmlData.DeclaredQuantity);
      xmlData.DeclaredQuantity -= _DeclaredQntty;
      this.TobaccoNotAllocated = Convert.ToDouble(this.TobaccoNotAllocated.DecimalValue() - _2Dispose);
      CustomsWarehouseDisposal _new = new CustomsWarehouseDisposal()
      {
        Archival = false,
        ClearingType = ClearingType.PartialWindingUp,
        CustomsStatus = CustomsStatus.NotStarted,
        CustomsProcedure = parent.ClearenceProcedure.Value.Convert2String(),
        CW_AddedKg = 0, //Assigned in SettledNetMass
        CW_DeclaredNetMass = _DeclaredQntty.DoubleValue(),
        CW_SettledNetMass = 0, //Assigned in SettledNetMass
        CW_SettledGrossMass = 0, //Assigned in SettledNetMass
        CW_PackageToClear = _Boxes.DoubleValue(),
        TobaccoValue = 0, //Assigned in SettledNetMass
        CWL_CWDisposal2DisposalRequestLibraryID = parent,
        CWL_CWDisposal2PCNTID = this.CWL_CW2PCNID,        
        CWL_CWDisposal2CustomsWarehouseID = this,
        SADDate = CAS.SharePoint.Extensions.SPMinimum,
        SKUDescription = xmlData.SKUDescription,
        CW_WZNoSupplemented = false,
        Title = "ToDo",
      };
      _new.CalculateMassValu(_2Dispose.DoubleValue());
      xmlData.AdditionalQuantity -= Convert.ToDecimal(_new.CW_AddedKg.Value);
      entities.CustomsWarehouseDisposal.InsertOnSubmit(_new);
      _new.UpdateTitle();
    }
    #endregion
    
  }
}
