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
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Customs Warehousing Account Class
  /// </summary>
  public partial class CustomsWarehouse
  {
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
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      Title = String.Format("CW-{0:D4}{1:D6}", this.CWC_EntryDate.Value.Year, Id.Value);
    }
    internal static bool RecordExist(Entities entities, string documentNo)
    {
      return (from CustomsWarehouse _cwx in entities.CustomsWarehouse where _cwx.DocumentNo.Contains(documentNo) select _cwx).Any();
    }
    public static void Create(Entities entities, string batch, DisposalRequestLib parent, CustomsWarehouseDisposal.XmlData _xmlData)
    {
      Dictionary<string, IGrouping<string, CustomsWarehouse>> _cwList = (from _cwi in entities.CustomsWarehouse
                                                                         where _cwi.AccountBalance.Value >= 0 && _cwi.Batch == batch
                                                                         group _cwi by _cwi.Batch into _cwGroup
                                                                         select _cwGroup
                                                ).ToDictionary<IGrouping<string, CustomsWarehouse>, string>(x => x.Key);
      Create(entities, parent, _cwList[batch], ref _xmlData);
    }
    private static void Create(Entities entities, DisposalRequestLib parent, IGrouping<string, CustomsWarehouse> grouping, ref CustomsWarehouseDisposal.XmlData _xmlData)
    {
      IOrderedEnumerable<CustomsWarehouse> _cwList = grouping.OrderBy(x => x.Id.Value);
      foreach (CustomsWarehouse _cwx in _cwList)
      {
        _cwx.Dispose(entities, parent, ref _xmlData);
        if (_xmlData.DeclaredQuantity + _xmlData.AdditionalQuantity <= 0)
          break;
      }
    }
    private void Dispose(Entities entities, DisposalRequestLib parent, ref CustomsWarehouseDisposal.XmlData _xmlData)
    {
      double _2Get = 0;
      double _AddedKg = 0;
      double _DeclaredNetMass = 0;
      double _2Dispose = _xmlData.DeclaredQuantity + _xmlData.AdditionalQuantity;
      if (this.TobaccoNotAllocated < _2Dispose)
        _2Dispose = this.TobaccoNotAllocated.Value;
      double _boxes = Math.Round(_2Dispose / this.CW_MassPerPackage.Value + 0.5, 0);
      double _2DisposeFromBoxes = _boxes * this.CW_MassPerPackage.Value;
      if (Math.Abs(_2DisposeFromBoxes - this.TobaccoNotAllocated.Value) < this.CW_MassPerPackage.Value)
        _2DisposeFromBoxes = this.TobaccoNotAllocated.Value;
      _DeclaredNetMass = Math.Min(_2DisposeFromBoxes, _xmlData.DeclaredQuantity);
      _AddedKg = Math.Min(_2DisposeFromBoxes - _DeclaredNetMass, _xmlData.AdditionalQuantity);

      CustomsWarehouseDisposal _new = new CustomsWarehouseDisposal()
      {
        AccountClosed = false,
        Archival = false,
        Batch = this.Batch,
        Currency = this.Currency,
        CustomsStatus = CustomsStatus.NotStarted,
        CW_AddedKg = _AddedKg,
        CW_DeclaredNetMass = _DeclaredNetMass,
        CW_SettledNetMass = _2Get,
        CW_SettledGrossMass = _2Get + BoxWeight * _boxes,
        CW_PackageToClear = _boxes,
        CWL_CWDisposal2DisposalRequestLibraryID = parent,
        CWL_CWDisposal2PCNTID = this.CWL_CW2PCNID,
        Grade = this.Grade,
        SKU = this.SKU,
        SKUDescription = _xmlData.SKUDescription,
        Title = "ToDo",
        TobaccoName = this.TobaccoName,//TODO secondary lookup
      };
    }

    private decimal DecVAlue(double? nullable)
    {
      return Convert.ToDecimal(nullable.Value);
    }

    public double BoxWeight { get; set; }
  }
}
