//<summary>
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
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Entity partial class of CustomsWarehouseDisposal.
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {
    #region public
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
      return Settings.FormatGoodsName(entities, _cw.TobaccoName, _cw.Grade, _cw.SKU, _cw.Batch, this.ClearingType.Value, _cw.DocumentNo);
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
        List<CustomsWarehouseDisposal> _Finished = (from _dsp in this.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal
                                                    where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                                                    select _dsp).ToList<CustomsWarehouseDisposal>();
        if (_Finished.Count<CustomsWarehouseDisposal>() == 0)
          this.No = 1;
        else
          this.No = _Finished.Max<CustomsWarehouseDisposal>(dspsl => dspsl.No.Value) + 1;
        AssignSADGood(sadGood);
        decimal _balance = CalculateRemainingQuantity();
        if (_balance == 0)
        {
          this.CW_RemainingPackage = 0;
          this.CW_RemainingTobaccoValue = 0;
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        }
        else
        {
          double _value = _Finished.Sum<CustomsWarehouseDisposal>(x => x.TobaccoValue.Value);
          double _pckgs = _Finished.Sum<CustomsWarehouseDisposal>(x => x.CW_PackageToClear.Value);
          this.CW_RemainingPackage = this.CWL_CWDisposal2CustomsWarehouseID.CW_PackageUnits - _pckgs - this.CW_PackageToClear;
          this.CW_RemainingTobaccoValue = this.CWL_CWDisposal2CustomsWarehouseID.Value - _value - this.TobaccoValue;
          this.ClearingType = Linq.ClearingType.PartialWindingUp;
        }
        CheckCNCosistency();
        this.CustomsStatus = Linq.CustomsStatus.Finished;
      }
      catch (Exception)
      {
        throw;
      }
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle()
    {
      try
      {
        string _numTxt = this.Id.HasValue ? String.Format("{0:D6}", this.Id.Value) : "XXXXXX";
        DateTime _entry = this.CWL_CWDisposal2CustomsWarehouseID != null ? this.CWL_CWDisposal2CustomsWarehouseID.CustomsDebtDate.Value : Extensions.DateTimeNull;
        Title = String.Format("CW-{0:D4}-{1}", _entry.Year, _numTxt);
      }
      catch (Exception) { }
    }
    #endregion

    #region private
    partial void OnLoaded()
    {
      this.PropertyChanging += CustomsWarehouseDisposal_PropertyChanging;
    }
    private void CustomsWarehouseDisposal_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
      UpdateTitle();
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
      this.DutyPerSettledAmount = _duties.DoubleValue();
      this.VATPerSettledAmount = _vat.DoubleValue();
      this.DutyAndVAT = (_vat + _duties).DoubleValue();
    }
    private decimal CalculateRemainingQuantity()
    {
      decimal _balance = this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance.DecimalValue() - this.CW_SettledNetMass.DecimalValue();
      this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance = this.RemainingQuantity = _balance.DoubleValue();
      return _balance;
    }
    #endregion

  }
}
