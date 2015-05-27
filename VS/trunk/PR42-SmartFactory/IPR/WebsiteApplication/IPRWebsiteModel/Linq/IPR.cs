//<summary>
//  Title   : class IPR
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Logging;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Class IPR - entity representing IPR account. This class cannot be inherited.
  /// </summary>
  public sealed partial class IPR
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IPR" /> class.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="iprdata">The _iprdata.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="declaration">The declaration.</param>
    public IPR(Entities entities, CWInterconnection.IPRAccountData iprdata, Clearence clearence, SADDocumentType declaration)
      : this()
    {
      Linq.Consent _consentLookup = GetAtIndex<Consent>(entities.Consent, iprdata.ConsentLookup);
      AccountClosed = false;
      AccountBalance = iprdata.NetMass;
      Archival = false;
      Batch = iprdata.BatchId;
      Cartons = iprdata.CartonsMass;
      ClearenceIndex = clearence;
      ClosingDate = CAS.SharePoint.Extensions.SPMinimum;
      ConsentPeriod = _consentLookup.ConsentPeriod;
      Currency = declaration.Currency;
      CustomsDebtDate = iprdata.CustomsDebtDate;
      DocumentNo = clearence.DocumentNo;
      Duty = iprdata.Duty;
      DutyName = iprdata.DutyName;
      Grade = iprdata.GradeName;
      GrossMass = iprdata.GrossMass;
      InvoiceNo = iprdata.Invoice;
      IPRDutyPerUnit = iprdata.DutyPerUnit;
      IPRLibraryIndex = null;
      IPR2ConsentTitle = _consentLookup;
      IPR2PCNPCN = GetAtIndex<PCNCode>(entities.PCNCode, iprdata.PCNTariffCodeLookup);
      IPRUnitPrice = iprdata.UnitPrice;
      IPRVATPerUnit = iprdata.VATPerUnit;
      this.IPR2JSOXIndex = null;
      NetMass = iprdata.NetMass;
      OGLValidTo = iprdata.ValidToDate;
      ProductivityRateMax = _consentLookup.ProductivityRateMax;
      ProductivityRateMin = _consentLookup.ProductivityRateMin;
      SKU = iprdata.SKU;
      TobaccoName = iprdata.TobaccoName;
      TobaccoNotAllocated = iprdata.NetMass;
      Title = "-- creating -- ";
      Value = iprdata.Value;
      VATName = iprdata.VATName;
      VAT = iprdata.VAT;
      ValidFromDate = _consentLookup.ValidFromDate;
      ValidToDate = _consentLookup.ValidToDate;
      if (iprdata.CartonsMass > 0)
        AddDisposal(entities, Convert.ToDecimal(iprdata.CartonsMass));
    }
    #endregion

    #region public
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      Title = String.Format("IPR-{0:D4}{1:D6}", DateTime.Today.Year, Id.Value);
    }
    /// <summary>
    /// Reverts the withdraw.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void RevertWithdraw(double quantity)
    {
      this.TobaccoNotAllocated += quantity;
    }
    /// <summary>
    /// Adds the disposal.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="quantity">The quantity.</param>
    /// <exception cref="CAS">CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal;_qunt &gt; 0;null</exception>
    public void AddDisposal(Entities edc, decimal quantity)
    {
      AddDisposal(edc, DisposalEnum.Cartons, ref quantity);
    }
    /// <summary>
    /// Insert on submit the disposal.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="clearance">The clearance.</param>
    /// <exception cref="CAS">CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal;_qunt > 0;null</exception>
    public void AddDisposal(Entities edc, decimal quantity, Clearence clearance)
    {
      Disposal _dsp = AddDisposal(edc, DisposalEnum.Tobacco, ref quantity);
      _dsp.Clearance = clearance;
      if (quantity > 0)
      {
        string _msg = String.Format("Cannot add Disposal to IPR  {0} because there is not material on the IPR.", this.Id.Value);
        throw CAS.SharePoint.Web.GenericStateMachineEngine.ActionResult.Exception
          (new CAS.SharePoint.ApplicationError("CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal", "_qunt > 0", _msg, null), _msg);
      }
    }
    /// <summary>
    /// Gets the VAT as decimal value.
    /// </summary>
    /// <value>
    /// The VAT.
    /// </value>
    public decimal VATDec { get { return Convert.ToDecimal(this.VAT.Value); } }
    /// <summary>
    /// Gets the duty as decimal value.
    /// </summary>
    /// <value>
    /// The duty.
    /// </value>
    public decimal DutyDec { get { return Convert.ToDecimal(this.Duty.Value); } }
    /// <summary>
    /// Return all Disposals associated with this item.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> object.</param>
    /// <param name="trace">The trace action.</param>
    /// <returns>All Disposals associated with this item</returns>
    public IEnumerable<Disposal> Disposals(Entities edc, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering IPR.Disposals", 147, TraceSeverity.Verbose);
      if (!this.Id.HasValue)
        return null;
      if (m_Disposals == null)
      {
        trace("IPR.Disposals reverse lookup calculation.", 151, TraceSeverity.Verbose);
        m_Disposals = from _dsx in edc.Disposal
                      let _idx = _dsx.Disposal2IPRIndex.Id.Value
                      where _idx == this.Id.Value
                      select _dsx;
        trace(String.Format("IPR.Disposals found reverse lookups {0}.", m_Disposals.Count()), 157, TraceSeverity.Verbose);
      }
      return m_Disposals;
    }
    /// <summary>
    /// Check if there are conditions to close the account - all entries must be cleared through customs.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="trace">The trace action.</param>
    /// <returns><c>true</c> if all accounts are in state finished or the <see cref="Disposal.SettledQuantityDec"/> is equal 0, <c>false</c> otherwise.</returns>
    public bool AllEntriesClosed(Entities edc, NamedTraceLogger.TraceAction trace)
    {
      return ! Disposals(edc, (x, y, z) => trace(x, y, z)).Where<Disposal>(v => v.SettledQuantityDec > 0 && v.CustomsStatus.Value != CustomsStatus.Finished).Any<Disposal>();
    }

    #region static
    /// <summary>
    /// Check if record exists.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/></param>
    /// <param name="documentNo">The document number.</param>
    /// <returns></returns>
    public static bool RecordExist(Entities edc, string documentNo)
    {
      return (from IPR _iprx in edc.IPR where _iprx.DocumentNo.Contains(documentNo) select _iprx).Any();
    }
    /// <summary>
    /// Gets the introducing quantity.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="dateStart">The date start.</param>
    /// <param name="dateEnd">The date end.</param>
    /// <returns></returns>
    public static decimal GetIntroducingData(Entities edc, JSOXLib parent, out DateTime dateStart, out DateTime dateEnd)
    {
      decimal _ret = 0;
      dateEnd = LinqIPRExtensions.DateTimeMinValue;
      dateStart = LinqIPRExtensions.DateTimeMaxValue;
      foreach (IPR _iprx in parent.IPR(edc))
      {
        _ret += _iprx.NetMassDec;
        dateEnd = LinqIPRExtensions.Max(_iprx.CustomsDebtDate.Value.Date, dateEnd);
        dateStart = LinqIPRExtensions.Min(_iprx.CustomsDebtDate.Value.Date, dateStart);
      }
      foreach (IPR _iprx in IPR.GetAllNew4JSOX(edc))
      {
        _iprx.IPR2JSOXIndex = parent;
        _ret += _iprx.NetMassDec;
        dateEnd = LinqIPRExtensions.Max(_iprx.CustomsDebtDate.Value.Date, dateEnd);
        dateStart = LinqIPRExtensions.Min(_iprx.CustomsDebtDate.Value.Date, dateStart);
      }
      return _ret;
    }
    /// <summary>
    /// Gets the current situation.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    public static decimal GetCurrentSituationData(Entities edc)
    {
      decimal _ret = 0;
      foreach (IPR _iprx in IPR.GetAllOpen4JSOX(edc))
        _ret += _iprx.AccountBalanceDec;
      return _ret;
    }
    #endregion

    #endregion

    #region internal
    internal decimal NetMassDec { get { return Convert.ToDecimal(this.NetMass.Value); } }
    internal enum ValueKey
    {
      //NotStarted
      TobaccoInFGCSNotStarted,
      WasteCSNotStarted,
      PureTobaccoCSNotStarted,
      DustCSNotStarted,
      OveruseCSNotStarted,
      SHMentholCSNotStarted,
      //CSStarted
      DustCSStarted,
      OveruseCSStarted,
      PureTobaccoCSStarted,
      SHMentholCSStarted,
      WasteCSStarted,
      TobaccoInFGCSStarted,
      //CSFinished
      TobaccoCSFinished,

      //calculated
      /// <summary>
      /// The tobacco started = sum of CSStarted
      /// </summary>
      TobaccoStarted,
      IPRBook,
      TobaccoAvailable,
      TobaccoEnteredIntoIPR,
      SHWasteOveruseCSNotStarted,
      TobaccoToBeUsedInTheProduction,
      TobaccoUsedInTheProduction,
    }
    internal class Balance : Dictionary<IPR.ValueKey, decimal>
    {
      #region creator
      internal Balance(Entities edc, IPR record, NamedTraceLogger.TraceAction trace)
      {
        trace("Entering IPR.Balance", 421, TraceSeverity.Verbose);
        foreach (ValueKey _vkx in Enum.GetValues(typeof(ValueKey)))
          base[_vkx] = 0;
        #region totals
        foreach (Disposal _dspx in record.Disposals(edc, trace))
        {
          switch (_dspx.CustomsStatus.Value)
          {
            case CustomsStatus.NotStarted:
              switch (_dspx.DisposalStatus.Value)
              {
                case DisposalStatus.Dust:
                  base[ValueKey.DustCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.SHMenthol:
                  base[ValueKey.SHMentholCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Waste:
                  base[ValueKey.WasteCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Overuse:
                  base[ValueKey.OveruseCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Tobacco:
                  base[ValueKey.PureTobaccoCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.TobaccoInCigaretes:
                  base[ValueKey.TobaccoInFGCSNotStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Cartons:
                case DisposalStatus.TobaccoInCigaretesDestinationEU:
                case DisposalStatus.TobaccoInCigaretesProduction:
                case DisposalStatus.TobaccoInCutfiller:
                case DisposalStatus.TobaccoInCutfillerDestinationEU:
                case DisposalStatus.TobaccoInCutfillerProduction:
                  break;
              }
              break;
            case CustomsStatus.Started:
              switch (_dspx.DisposalStatus.Value)
              {
                case DisposalStatus.Dust:
                  base[ValueKey.DustCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.SHMenthol:
                  base[ValueKey.SHMentholCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Waste:
                  base[ValueKey.WasteCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Overuse:
                  base[ValueKey.OveruseCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Tobacco:
                  base[ValueKey.PureTobaccoCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.TobaccoInCigaretes:
                  base[ValueKey.TobaccoInFGCSStarted] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Cartons:
                case DisposalStatus.TobaccoInCigaretesDestinationEU:
                case DisposalStatus.TobaccoInCigaretesProduction:
                case DisposalStatus.TobaccoInCutfiller:
                case DisposalStatus.TobaccoInCutfillerDestinationEU:
                case DisposalStatus.TobaccoInCutfillerProduction:
                  break;
              }
              break;
            case CustomsStatus.Finished:
              switch (_dspx.DisposalStatus.Value)
              {
                case DisposalStatus.Cartons:
                  break;
                case DisposalStatus.Dust:
                case DisposalStatus.SHMenthol:
                case DisposalStatus.Waste:
                case DisposalStatus.Overuse:
                case DisposalStatus.Tobacco:
                case DisposalStatus.TobaccoInCigaretes:
                case DisposalStatus.TobaccoInCigaretesDestinationEU:
                case DisposalStatus.TobaccoInCigaretesProduction:
                case DisposalStatus.TobaccoInCutfiller:
                case DisposalStatus.TobaccoInCutfillerDestinationEU:
                case DisposalStatus.TobaccoInCutfillerProduction:
                  base[ValueKey.TobaccoCSFinished] += _dspx.SettledQuantityDec;
                  break;
              }
              break;
          }
        }
        #endregion
        base[ValueKey.TobaccoEnteredIntoIPR] = record.NetMassDec;
        base[ValueKey.IPRBook] = IPRBook;
        base[ValueKey.TobaccoStarted] = TobaccoStarted;
        base[ValueKey.SHWasteOveruseCSNotStarted] = SHWasteOveruseCSNotStarted;
        base[ValueKey.TobaccoAvailable] = TobaccoAvailable;
        base[ValueKey.TobaccoUsedInTheProduction] = TobaccoUsedInTheProduction;
        base[ValueKey.TobaccoToBeUsedInTheProduction] = base[ValueKey.TobaccoEnteredIntoIPR] - base[ValueKey.TobaccoUsedInTheProduction];
      }
      #endregion

      #region internal
      internal new double this[ValueKey index]
      {
        get { return Convert.ToDouble(base[index]); }
      }
      internal Dictionary<IPR.ValueKey, decimal> Base { get { return this; } }
      #endregion

      #region private
      private decimal IPRBook
      {
        get
        {
          return base[ValueKey.TobaccoEnteredIntoIPR] - base[ValueKey.TobaccoCSFinished];
        }
      }
      private decimal TobaccoStarted
      {
        get
        {
          return
            base[ValueKey.TobaccoInFGCSStarted] +
            base[ValueKey.DustCSStarted] +
            base[ValueKey.WasteCSStarted] +
            base[ValueKey.SHMentholCSStarted] +
            base[ValueKey.OveruseCSStarted] +
            base[ValueKey.PureTobaccoCSStarted];
        }
      }
      private decimal SHWasteOveruseCSNotStarted
      {
        get
        {
          return
            base[ValueKey.WasteCSNotStarted] +
            base[ValueKey.SHMentholCSNotStarted] +
            base[ValueKey.OveruseCSNotStarted] +
            base[ValueKey.PureTobaccoCSNotStarted];
        }
      }
      private decimal TobaccoAvailable
      {
        get
        {
          return
            base[ValueKey.IPRBook] -
            base[ValueKey.TobaccoStarted] -
            base[ValueKey.SHWasteOveruseCSNotStarted] -
            base[ValueKey.DustCSNotStarted];
        }
      }
      private decimal TobaccoUsedInTheProduction
      {
        get
        {
          return
            base[ValueKey.TobaccoInFGCSNotStarted] +
            base[ValueKey.DustCSNotStarted] +
            base[ValueKey.SHWasteOveruseCSNotStarted] +
            base[ValueKey.TobaccoCSFinished] +
            base[ValueKey.TobaccoStarted];
        }
      }
      #endregion

    }
    internal void AddDisposal(Entities edc, DisposalEnum kind, ref decimal toDispose, Material material, InvoiceContent invoiceContent, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering IPR.AddDisposal", 421, TraceSeverity.Verbose);
      Disposal _dsp = AddDisposal(edc, kind, ref toDispose);
      _dsp.Material = material;
      _dsp.ClearThroughCustom(invoiceContent, _x => this.RecalculateLastStarted(edc, _x, trace));
      SADGood _sg = invoiceContent.InvoiceIndex.ClearenceIndex.Clearence2SadGoodID;
      if (_sg != null)
        _dsp.FinishClearingThroughCustoms(edc, _sg, trace);
    }
    internal void AddDisposal(Entities edc, DisposalEnum _kind, ref decimal _toDispose, Material material)
    {
      Disposal _dsp = AddDisposal(edc, _kind, ref _toDispose);
      _dsp.Material = material;
    }
    internal decimal TobaccoNotAllocatedDec { get { return Convert.ToDecimal(this.TobaccoNotAllocated.Value); } set { this.TobaccoNotAllocated = Convert.ToDouble(value); } }
    /// <summary>
    /// Withdraws the specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <param name="max">The maximum quantity to be reverted to the record if <paramref name="quantity"/> is less then 0.</param>
    /// <returns></returns>
    internal decimal Withdraw(ref decimal quantity, decimal max)
    {
      decimal _toDispose = 0;
      if (quantity >= 0)
        _toDispose = Math.Min(quantity, this.TobaccoNotAllocatedDec);
      else
        _toDispose = Math.Max(quantity, -max);
      this.TobaccoNotAllocatedDec -= _toDispose;
      quantity -= _toDispose;
      return _toDispose;
    }
    internal void RecalculateClearedRecords(Entities edc, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering IPR.RecalculateClearedRecords", 453, TraceSeverity.Verbose);
      if (this.AccountClosed.Value)
      {
        trace("ApplicationException at IPR.RecalculateClearedRecords - closed account", 455, TraceSeverity.High);
        throw new ApplicationException("IPR.RecalculateClearedRecords cannot be executed for closed account");
      }
      IEnumerable<Disposal> _2Calculate = this.Disposals(edc, trace);
      _2Calculate = (from _dx in _2Calculate where _dx.CustomsStatus.Value == Linq.CustomsStatus.Finished orderby _dx.SPNo.Value ascending select _dx).ToList<Disposal>();
      this.AccountBalance = this.NetMass;
      foreach (Disposal _dx in _2Calculate)
        _dx.CalculateRemainingQuantity();
    }
    internal static IQueryable<IGrouping<string, IPR>> GetAllOpen4JSOXGroups(Entities edc)
    {
      return from _iprx in edc.IPR
             where !_iprx.AccountClosed.Value
             orderby _iprx.CustomsDebtDate.Value ascending
             group _iprx by _iprx.Batch;
    }
    internal void RecalculateLastStarted(Entities edc, Linq.Disposal disposal, NamedTraceLogger.TraceAction trace)
    {
      if (this.TobaccoNotAllocatedDec != 0 || _disposal.Where(x => x.SettledQuantityDec > 0 && x.CustomsStatus == CustomsStatus.NotStarted).Any())
        return;
      trace("Starting IPR.RecalculateLastStarted", 52, TraceSeverity.Verbose);
      IEnumerable<Disposal> _dspsl = Disposals(edc, trace);
      disposal.DutyPerSettledAmount = (disposal.DutyPerSettledAmount + this.Duty.Value - (from _dec in _dspsl where _dec.DutyPerSettledAmount.HasValue select _dec.DutyPerSettledAmount.Value).Sum(itm => itm)).Value.Round2Decimals();
      disposal.VATPerSettledAmount = (disposal.VATPerSettledAmount + this.VAT.Value - (from _dec in _dspsl where _dec.VATPerSettledAmount.HasValue select _dec.VATPerSettledAmount.Value).Sum(itm => itm)).Value.Round2Decimals();
      disposal.DutyAndVAT = (disposal.DutyPerSettledAmount + disposal.VATPerSettledAmount).Value.Round2Decimals();
      disposal.TobaccoValue += this.Value.Value - (from _dec in _dspsl where _dec.TobaccoValue.HasValue select _dec.TobaccoValue.Value).Sum(itm => itm); ;
    }
    #endregion

    #region private
    IEnumerable<Disposal> m_Disposals = null;
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    private Disposal AddDisposal(Entities edc, DisposalEnum status, ref decimal quantity)
    {
      Linq.DisposalStatus _typeOfDisposal = Entities.GetDisposalStatus(status);
      decimal _toDispose = 0;
      if (status == DisposalEnum.Cartons)
      {
        _toDispose = quantity;
        quantity = 0;
      }
      else
        _toDispose = Withdraw(ref quantity, 0);
      Disposal _newDisposal = new Disposal(this, _typeOfDisposal, _toDispose);
      edc.Disposal.InsertOnSubmit(_newDisposal);
      return _newDisposal;
    }
    private static IQueryable<IPR> GetAllNew4JSOX(Entities edc)
    {
      return from _iprx in edc.IPR where _iprx.IPR2JSOXIndex == null select _iprx;
    }
    /// <summary>
    /// Gets all open account.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    private static IQueryable<IPR> GetAllOpen4JSOX(Entities edc)
    {
      return from _iprx in edc.IPR
             where !_iprx.AccountClosed.Value
             orderby _iprx.CustomsDebtDate.Value
             select _iprx;
    }
    private decimal AccountBalanceDec { get { return Convert.ToDecimal(AccountBalance.Value); } }
    #endregion

  }
}
