//<summary>
//  Title   : Inward Processing Account Record Data
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

using CAS.SharePoint.Logging;
using CAS.SmartFactory.Customs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection
{

  /// <summary>
  /// Inward Processing Account Record Data
  /// </summary>
  public class IPRAccountData : AccountData
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRAccountData"/> class.
    /// </summary>
    /// <param name="clearanceLookup">The clearance lookup.</param>
    public IPRAccountData(int clearanceLookup)
      : base(clearanceLookup)
    { }
    #endregion

    #region IPR Data
    internal double CartonsMass { get; private set; }
    internal double Duty { get; private set; }
    internal string DutyName { get; private set; }
    internal double DutyPerUnit { get; private set; }
    internal string VATName { get; private set; }
    internal double VAT { get; private set; }
    internal double VATPerUnit { get; private set; }
    internal double UnitPrice { get; private set; }
    internal double Value { get; private set; }
    internal DateTime ValidToDate { get; private set; }
    #endregion

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountData" /> class.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> object.</param>
    /// <param name="clearance">The clearance.</param>
    /// <param name="messageType">Type of the customs message.</param>
    /// <param name="warnings">The list of warnings.</param>
    /// <param name="trace">The trace.</param>
    public override void GetAccountData(Entities edc, Clearence clearance, Customs.Account.CommonAccountData.MessageType messageType, List<Warnning> warnings, NamedTraceLogger.TraceAction trace)
    {
      base.GetAccountData(edc, clearance, messageType, warnings, trace);
      Value = clearance.Clearence2SadGoodID.TotalAmountInvoiced.GetValueOrDefault(0);
      UnitPrice = Value / NetMass;
      AnalizeDutyAndVAT(edc, clearance.Clearence2SadGoodID);
    }
    /// <summary>
    /// Calls the remote service.
    /// </summary>
    /// <param name="requestUrl">The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="warningList">The warning list.</param>
    public override void CallService(string requestUrl, System.Collections.Generic.List<Customs.Warnning> warningList)
    {
      //throw new NotImplementedException();
    }
    #endregion

    #region private
    private static int m_DaysPerMath = 30;
    private void AnalizeDutyAndVAT(Entities edc, SADGood good)
    {
      string _at = "Started";
      try
      {
        Duty = 0;
        VAT = 0;
        DutyName = string.Empty;
        VATName = string.Empty;
        foreach (SADDuties _duty in good.SADDuties(edc))
        {
          _at = "switch " + _duty.DutyType;
          switch (_duty.DutyType)
          {
            //Duty
            case "A10":
            case "A00":
            case "A20":
              Duty += _duty.Amount.Value;
              _at = "DutyName";
              DutyName += String.Format("{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value);
              break;
            //VAT
            case "B00":
            case "B10":
            case "B20":
              VAT += _duty.Amount.Value;
              _at = "VATName";
              VATName += String.Format("{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value);
              break;
            default:
              break;
          }
        }
        _at = "DutyPerUnit";
        DutyPerUnit = Duty / NetMass;
        _at = "VATPerUnit";
        VATPerUnit = VAT / NetMass;
      }
      catch (Exception _ex)
      {
        string _src = String.Format("AnalizeDutyAndVAT error at {0}", _at);
        throw new IPRDataConsistencyException(_src, _ex.Message, _ex, _src);
      }
    }
    /// <summary>
    /// Analyzes the good.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="good">The good.</param>
    /// <param name="_messageType">Type of the _message.</param>
    protected internal override void AnalizeGood(Entities edc, SADGood good, MessageType _messageType)
    {
      base.AnalizeGood(edc, good, _messageType);
      SADPackage _packagex = good.SADPackage(edc).First();
      if (_packagex.Package.ToUpper().Contains("CT"))
        CartonsMass = GrossMass - NetMass;
      else
        CartonsMass = 0;
    }
    /// <summary>
    /// Gets the process.
    /// </summary>
    /// <value>
    /// The process.
    /// </value>
    protected override CustomsProcess Process
    {
      get { return CustomsProcess.cw; }
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> instance</param>
    /// <param name="good">The good.</param>
    protected internal override void GetNetMass(Entities edc, SADGood good)
    {
      SADQuantity _quantity = good.SADQuantity(edc).FirstOrDefault();
      NetMass = _quantity == null ? 0 : _quantity.NetMass.GetValueOrDefault(0);
    }
    /// <summary>
    /// Sets the valid to date.
    /// </summary>
    /// <param name="customsDebtDate"></param>
    /// <param name="consent"></param>
    protected internal override void SetValidToDate(DateTime customsDebtDate, Consent consent)
    {
      ValidToDate = customsDebtDate + TimeSpan.FromDays(consent.ConsentPeriod.Value * m_DaysPerMath);
    }
    #endregion

  }
}
