﻿//<summary>
//  Title   : Name of Application
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
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Entity partial class of CustomsWarehouseDisposal.
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {
    /// <summary>
    /// CustomsWarehouseDisposal data obtained from xml message.
    /// </summary>
    public struct XmlData
    {
      /// <summary>
      /// Additional quantity declared to dispose
      /// </summary>
      public decimal AdditionalQuantity;
      /// <summary>
      /// The declared quantity
      /// </summary>
      public decimal DeclaredQuantity;
      /// <summary>
      /// The sku description
      /// </summary>
      public string SKUDescription;
    }
    /// <summary>
    /// Goodses the name.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public string GoodsName(Entities entities)
    {
      CustomsWarehouse _cw = CWL_CWDisposal2CustomsWarehouseID;
      return Settings.FormatGoodsName(entities, _cw.TobaccoName, _cw.Grade, _cw.SKU, _cw.Batch, _cw.DocumentNo);
    }
    /// <summary>
    /// Gets the goods code.
    /// </summary>
    /// <value>
    /// The goods code.
    /// </value>
    public string ProductCode
    {
      get
      {
        string _code = this.CWL_CWDisposal2PCNTID.ProductCodeNumber;
        return _code.Substring(_code.Length - 2, 2);
      }
    }
    /// <summary>
    /// Gets the taric.
    /// </summary>
    /// <value>
    /// The taric.
    /// </value>
    public string ProductCodeTaric
    {
      get
      {
        string _code = this.CWL_CWDisposal2PCNTID.ProductCodeNumber;
        return _code.Substring(0, _code.Length - 2);
      }
    }
    internal void FinishClearThroughCustoms(SADGood sadGood)
    {
      if (this.CustomsStatus.Value == Linq.CustomsStatus.Finished)
        return;
      try
      {
        IQueryable<CustomsWarehouseDisposal> _Finished = from _dsp in this.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal
                                                         where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                                                         select _dsp;
        if (_Finished.Count<CustomsWarehouseDisposal>() == 0)
          this.No = 1;
        else
          this.No = _Finished.Max<CustomsWarehouseDisposal>(dspsl => dspsl.No.Value) + 1;
        AssignSADGood(sadGood);
        decimal _balance = CalculateRemainingQuantity();
        if (_balance == 0)
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        else
          this.ClearingType = Linq.ClearingType.PartialWindingUp;
        CheckCNCosistency();
        this.CustomsStatus = Linq.CustomsStatus.Finished;
      }
      catch (Exception)
      {
        throw;
      }
    }
    private void CheckCNCosistency()
    {
      //TODO CheckCNCosistency NotImplementedException();
    }
    private void AssignSADGood(SADGood sadGood)
    {
      this.SADDate = sadGood.SADDocumentIndex.CustomsDebtDate;
      this.SADDocumentNo = sadGood.SADDocumentIndex.DocumentNumber;
      //TODO check consistency and generate warning.
      this.CustomsProcedure = sadGood.Procedure;
      this.TobaccoValue = sadGood.TotalAmountInvoiced;
      decimal _vat = 0;
      decimal _duties = 0;
      foreach (SADDuties _sdc in sadGood.SADDuties)
      {
        switch (Settings.DutyKind(_sdc.DutyType))
        {
          case Settings.DutyKindEnum.VAT:
            _vat += _sdc.Amount.DecimalValue();
            break;
          case Settings.DutyKindEnum.Duty:
            _duties += _sdc.Amount.DecimalValue();
            break;
          case Settings.DutyKindEnum.ExciseDuty:
            throw new NotImplementedException();
        }
      }
      this.DutyPerSettledAmount = (_duties / this.CW_SettledNetMass.DecimalValue()).DoubleValue();
      this.VATPerSettledAmount = (_vat / this.CW_SettledNetMass.DecimalValue()).DoubleValue();
      this.DutyAndVAT = (_vat + _duties).DoubleValue();
      this.CW_RemainingPackage -= this.CW_PackageToClear;
    }
    private decimal CalculateRemainingQuantity()
    {
      decimal _balance = _balance = this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance.DecimalValue() - this.CW_SettledNetMass.DecimalValue();
      this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance = this.RemainingQuantity = _balance.DoubleValue();
      return _balance;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle(DateTime dateTime)
    {
      Title = String.Format("CW-{0:D4}{1:D6}", dateTime.Year, "XXXXXX"); //TODO Id.Value);
    }
  }
}
