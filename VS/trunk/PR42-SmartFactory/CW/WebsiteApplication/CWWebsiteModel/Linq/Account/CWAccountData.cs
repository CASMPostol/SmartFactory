//<summary>
//  Title   : Customs Warehouse Account Record Data
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
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CAS.SharePoint;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data - registered as an external service
  /// </summary>
  public class CWAccountData : ICWAccountFactory
  {
    #region ctor
    public CWAccountData() { }
    #endregion

    #region properties
    internal CommonAccountData CommonAccountData { get; private set; }
    internal DateTime EntryDate { get; private set; }
    //lookup columns.
    internal Clearence ClearenceLookup { get; private set; }
    internal Consent ConsentLookup { get; private set; }
    internal Vendor VendorLookup { get; private set; }
    internal PCNCode PCNTariffCodeLookup { get; set; }
    //from good descriptions.
    /// <summary>
    /// Gets the precise goods quantity (nett amss) from the description.
    /// </summary>
    /// <value>
    /// The goods quantity in kg.
    /// </value>
    internal double? CWQuantity { get; private set; }
    /// <summary>
    /// Gets the units of mesure - usually kg .
    /// </summary>
    /// <value>
    /// The units.
    /// </value>
    internal string Units { get; private set; }
    /// <summary>
    /// Gets the weight of the boxws.
    /// </summary>
    /// <value>
    /// The boxes weight in kg.
    /// </value>
    internal double? CWPackageKg { get { return CommonAccountData.GrossMass - CWQuantity; } }
    /// <summary>
    /// Gets the number of boxes.
    /// </summary>
    /// <value>
    /// The number of units (boxes).
    /// </value>
    internal double? CWPackageUnits { get; private set; }
    /// <summary>
    /// Gets the mass per package (box).
    /// </summary>
    /// <value>
    /// The mass per package in kg.
    /// </value>
    internal double? CWMassPerPackage { get { return CWQuantity / CWPackageUnits; } }
    //from required documents.
    internal string CW_CertificateOfOrgin { get; private set; }
    internal string CW_CertificateOfAuthenticity { get; private set; }
    internal DateTime? CW_CODate { get; private set; }
    internal DateTime? CW_COADate { get; private set; }
    internal DateTime? ValidToDate { get; private set; }
    #endregion

    #region ICWAccountFactory Members
    /// <summary>
    /// Creates the Customs Warehousing account.
    /// </summary>
    /// <param name="accountData">The account data.</param>
    /// <param name="warnings">The warnings collection.</param>
    /// <param name="requestedUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    void ICWAccountFactory.CreateCWAccount(CommonAccountData accountData, List<Customs.Warnning> warnings, string requestedUrl)
    {
      string _at = "Beginning";
      try
      {
        using (Entities _edc = new Entities(requestedUrl))
        {
          if (Linq.CustomsWarehouse.RecordExist(_edc, accountData.DocumentNo))
          {
            string _msg = "CW record with the same SAD document number: {0} exist";
            throw new CreateCWAccountException(String.Format(_msg, accountData.DocumentNo));
          }
          _at = "CommonAccountData";
          this.CommonAccountData = accountData;
          _at = "ClearanceLookup";
          this.ClearenceLookup = Element.GetAtIndex<Clearence>(_edc.Clearence, accountData.ClearenceLookup);
          _at = "ConsentLookup";
          this.ConsentLookup = Element.GetAtIndex<Consent>(_edc.Consent, CommonAccountData.ConsentLookup);
          _at = "VendorLookup";
          VendorLookup = Vendor.FirstOrDefault(_edc);
          _at = "PCNTariffCodeLookup";
          PCNTariffCodeLookup = Element.GetAtIndex<PCNCode>(_edc.PCNCode, accountData.PCNTariffCodeLookup);
          _at = "AnalyzeGoodsDescription";
          AnalizeGoodsDescription(_edc, ClearenceLookup.Clearence2SadGoodID.GoodsDescription, warnings);
          _at = "AnalyzeGoodsDescription";
          AnalyzeCertificates(_edc, ClearenceLookup.Clearence2SadGoodID.SADRequiredDocuments(_edc, false), warnings);
          this.EntryDate = DateTime.Today;
          bool _fatal = (from _wx in warnings where _wx.Fatal select _wx).Any<Warnning>();
          if (_fatal)
            throw new CreateCWAccountException("There are fatal errors in the message - CW account is not created.");
          CustomsWarehouse _cw = new CustomsWarehouse(_edc, this);
          _at = "InsertOnSubmit";
          _edc.CustomsWarehouse.InsertOnSubmit(_cw);
          _at = "SubmitChanges #1";
          _edc.SubmitChanges();
          _at = "UpdateTitle";
          _cw.UpdateTitle();
          _at = "SubmitChanges #2";
          _edc.SubmitChanges();
        }
      }
      catch (CreateCWAccountException _ccwe)
      {
        Warnning _wrn = new Warnning(_ccwe.Message, true);
        warnings.Add(_wrn);
      }
      catch (Exception ex)
      {
        string _msg = string.Format(Properties.Resources.UnexpectedExceptionMessage, "ICWAccountFactory.CreateCWAccount", _at, ex.Message);
        Warnning _wrn = new Warnning(_msg, true);
        warnings.Add(_wrn);
      }
    }
    /// <summary>
    /// Clear through customs.
    /// </summary>
    /// <param name="commonClearanceData">The common clearance data.</param>
    /// <param name="warnings">The warnings.</param>
    /// <param name="requestedUrl">The requested URL.</param>
    void ICWAccountFactory.ClearThroughCustoms(CommonClearanceData commonClearanceData, List<Warnning> warnings, string requestedUrl)
    {
      WebsiteModelExtensions.TraceEvent("Starting CWAccountData.ICWAccountFactory.ClearThroughCustoms", 159, TraceSeverity.Verbose, WebsiteModelExtensions.LoggingCategories.CloseAccount);
      string _at = "Beginning";
      try
      {
        using (Entities _edc = new Entities(requestedUrl))
        {
          _at = "ClearanceLookup";
          Clearence _Clearance = Element.GetAtIndex<Clearence>(_edc.Clearence, commonClearanceData.ClearenceLookup);
          _Clearance.FinishClearThroughCustoms(_edc, (message, eventId, severity) => WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.CloseAccount));
          WebsiteModelExtensions.TraceEvent("ICWAccountFactory.ClearThroughCustoms at SubmitChanges", 168, TraceSeverity.Verbose, WebsiteModelExtensions.LoggingCategories.CloseAccount);
          _edc.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        string _msg = string.Format(Properties.Resources.UnexpectedExceptionMessage, "ICWAccountFactory.CreateCWAccount", _at, ex.Message);
        WebsiteModelExtensions.TraceEvent(_msg, 173, TraceSeverity.High, WebsiteModelExtensions.LoggingCategories.CloseAccount);
        Warnning _wrn = new Warnning(_msg, true);
        warnings.Add(_wrn);
      }
      WebsiteModelExtensions.TraceEvent("Finishing CWAccountData.ICWAccountFactory.ClearThroughCustoms", 179, TraceSeverity.Verbose, WebsiteModelExtensions.LoggingCategories.CloseAccount);
    }
    #endregion

    #region private
    private void AnalyzeCertificates(Entities entities, IEnumerable<SADRequiredDocuments> sadRequiredDocumentsEntitySet, List<Warnning> warnings)
    {
      List<string> _stringsList = new List<string>();
      foreach (SADRequiredDocuments _srdx in (from _dx in sadRequiredDocumentsEntitySet select _dx))
      {
        string _code = _srdx.Code.Trim().ToUpper();
        if (_code.Contains(Settings.CustomsProcedureCodeA004))
        {
          CW_CertificateOfAuthenticity = _srdx.Number.GetFirstCapture(Settings.GetParameter(entities, SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern), _na, _stringsList);
          CW_COADate = GetCertificateDate(entities, _srdx.Number, warnings);
        }
        else if (_code.Contains(Settings.CustomsProcedureCodeN865) || _code.Contains(Settings.CustomsProcedureCodeN954))
        {
          CW_CertificateOfOrgin = _srdx.Number.GetFirstCapture(Settings.GetParameter(entities, SettingsEntry.GoodsDescription_CertificateOfOrgin_Pattern), _na, _stringsList);
          CW_CODate = GetCertificateDate(entities, _srdx.Number, warnings);
        }
      }
      Convert2Warnings(warnings, _stringsList, false);
      ValidToDate = CalculateValidToDate(entities, CW_COADate, CW_CODate, warnings);
    }
    private DateTime? CalculateValidToDate(Entities entities, DateTime? CW_COADate, DateTime? CW_CODate, List<Warnning> warnings)
    {
      double _vtdIfNotProvided = 0; // in days
      double _validPeriod = _vtdIfNotProvided;
      if (!CW_COADate.HasValue && !CW_CODate.HasValue)
        return new Nullable<DateTime>();
      DateTime? _ret = DateTime.Today;
      _ret = CW_CODate.GetValueOrDefault(DateTime.Today) < _ret ? CW_CODate.GetValueOrDefault(DateTime.Today) : _ret;
      _ret = CW_COADate.GetValueOrDefault(DateTime.Today) < _ret ? CW_COADate.GetValueOrDefault(DateTime.Today) : _ret;
      if (!Double.TryParse(Settings.GetParameter(entities, SettingsEntry.DefaultValidToDatePeriod), out _validPeriod))
        _validPeriod = _vtdIfNotProvided; //TODO add warning
      return _ret + TimeSpan.FromDays(_validPeriod);
    }
    private DateTime? GetCertificateDate(Entities entities, string code, List<Warnning> warnings)
    {
      bool _severity = false;
      DateTime? _ret = new Nullable<DateTime>();
      string _at = "Matches";
      try
      {
        string _pattern = Settings.GetParameter(entities, SettingsEntry.LooselyFormatedDate);
        Match _result = Regex.Match(code, _pattern);
        int _cnt = _result.Groups.Count;
        if (_cnt < 4)
        {
          string _wrn = "Cannot recognize correct data format from the certificate {0} using pattern {1} - wrong number of date parts {2}";
          _wrn = String.Format(_wrn, code, _pattern, _cnt);
          warnings.Add(new Warnning(_wrn, _severity));
          return _ret;
        }
        _at = "yar";
        int _yar = int.Parse(_result.Groups[3].Value);
        _at = "month";
        int _month = int.Parse(_result.Groups[2].Value);
        _at = "day";
        int _day = int.Parse(_result.Groups[1].Value);
        _at = "new DateTime";
        _ret = new DateTime(_yar, _month, _day);
      }
      catch (Exception _ex)
      {
        string _mssg = String.Format("Cannot get data from the certificate '{0}' at {2} because of the error {1}", code, _ex.Message, _at);
        warnings.Add(new Warnning(_mssg, _severity));
      }
      return _ret;
    }
    private void AnalizeGoodsDescription(Entities edc, string goodsDescription, List<Warnning> warnings)
    {
      List<string> _stringsList = new List<string>();
      CWPackageUnits = Convert2Double(goodsDescription.GetFirstCapture(Settings.GetParameter(edc, SettingsEntry.GoodsDescription_CWPackageUnits_Pattern), _na, _stringsList), _stringsList);
      CWQuantity = Convert2Double(goodsDescription.GetFirstCapture(Settings.GetParameter(edc, SettingsEntry.GoodsDescription_CWQuantity_Pattern), _na, _stringsList), _stringsList);
      Units = goodsDescription.GetFirstCapture(Settings.GetParameter(edc, SettingsEntry.GoodsDescription_Units_Pattern), _na, _stringsList);
      Convert2Warnings(warnings, _stringsList, true);
    }
    private static void Convert2Warnings(List<Warnning> warningsList, List<string> stringsList, bool fatal)
    {
      if (stringsList.Count == 0)
        return;
      warningsList.AddRange(from _erx in stringsList select new Warnning(_erx, fatal));
    }
    private string _na = "Not recognized";
    private double? Convert2Double(string value, List<string> errors)
    {
      double _ret;
      if (Double.TryParse(value.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _ret))
        return _ret;
      errors.Add(String.Format("Coversion to float of the {0} failed.", value));
      return new Nullable<double>();
    }
    private class CreateCWAccountException : Exception
    {
      public CreateCWAccountException(string message) : base(message) { }
    }
    #endregion

  }
}
