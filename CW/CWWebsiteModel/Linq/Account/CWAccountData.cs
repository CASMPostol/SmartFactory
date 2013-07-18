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
using System.Linq;
using System.Text.RegularExpressions;
using CAS.SharePoint;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data - registered as an external service
  /// </summary>
  public class CWAccountData: ICWAccountFactory
  {
    #region ctor
    public CWAccountData() { }
    #endregion

    #region properties
    internal CommonAccountData CommonAccountData { get; private set; }
    internal DateTime? EntryDate { get; private set; }
    internal Clearence ClearenceLookup { get; private set; }
    internal Consent ConsentLookup { get; private set; }
    internal double? CWMassPerPackage { get { return CWQuantity / CWPackageUnits; } }
    //Good descriptionc
    internal double? CWQuantity { get; private set; }
    internal string Units { get; private set; }
    internal double? CWPackageKg { get { return CommonAccountData.GrossMass - CWQuantity; } }
    internal double? CWPackageUnits { get; private set; }
    //from Required documents.
    internal string CW_CertificateOfOrgin { get; private set; }
    internal string CW_CertificateOfAuthenticity { get; private set; }
    internal DateTime? CW_CODate { get; private set; }
    internal DateTime? CW_COADate { get; private set; }
    internal DateTime ValidToDate { get; private set; }
    #endregion

    #region ICWAccountFactory Members
    /// <summary>
    /// Creates the Customs Warehousing account.
    /// </summary>
    /// <param name="accountData">The account data.</param>
    /// <param name="warnings">The warnings collection.</param>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    void ICWAccountFactory.CreateCWAccount( CommonAccountData accountData, List<Customs.Warnning> warnings, string requestUrl )
    {
      string _at = "Beginning";
      try
      {
        using ( Entities _edc = new Entities( requestUrl ) )
        {
          if ( Linq.CustomsWarehouse.RecordExist( _edc, accountData.DocumentNo ) )
          {
            string _msg = "CW record with the same SAD document number: {0} exist";
            throw new CreateCWAccountException( String.Format( _msg, ClearenceLookup.DocumentNo ) );
          }
          _at = "ProcessCustomsMessage";
          this.CommonAccountData = accountData;
          _at = "GetAtIndex<Clearence>";
          this.ClearenceLookup = Element.GetAtIndex<Clearence>( _edc.Clearence, accountData.ConsentLookup );
          _at = "GetAtIndex<Consent>";
          this.ConsentLookup = Element.GetAtIndex<Consent>( _edc.Consent, CommonAccountData.ConsentLookup );
          _at = "AnalizeGoodsDescription";
          AnalizeGoodsDescription( _edc, ClearenceLookup.Clearence2SadGoodID.GoodsDescription, warnings );
          AnalyzeCertificates( _edc, ClearenceLookup.Clearence2SadGoodID.SADRequiredDocuments, warnings );
          this.EntryDate = DateTime.Today;
          CustomsWarehouse _cw = new CustomsWarehouse( _edc, this );
          _at = "InsertOnSubmit";
          _edc.CustomsWarehouse.InsertOnSubmit( _cw );
          _at = "SubmitChanges #1";
          _edc.SubmitChanges();
          _at = "UpdateTitle";
          _cw.UpdateTitle();
          _at = "SubmitChanges #2";
          _edc.SubmitChanges();
        }
      }
      catch ( CreateCWAccountException _ccwe )
      {
        Warnning _wrn = new Warnning( _ccwe.Message, true );
        warnings.Add( _wrn );
      }
      catch ( Exception ex )
      {
        string _msg = string.Format( Properties.Resources.UnexpectedExceptionMessage, "ICWAccountFactory.CreateCWAccount", _at, ex.Message );
        Warnning _wrn = new Warnning( _msg, true );
        warnings.Add( _wrn );
      }
    }
    #endregion

    #region private
    private void AnalyzeCertificates( Entities entities, EntitySet<SADRequiredDocuments> sadRequiredDocumentsEntitySet, List<Warnning> warnings )
    {
      List<string> _stringsList = new List<string>();
      foreach ( SADRequiredDocuments _srdx in ( from _dx in sadRequiredDocumentsEntitySet select _dx ) )
      {
        string _code = _srdx.Code.Trim().ToUpper();
        if ( _code.Contains( Settings.CustomsProcedureCodeA004 ) )
        {
          CW_CertificateOfAuthenticity = _srdx.Number.GetFirstCapture( Settings.GetParameter( entities, SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern ), _na, _stringsList );
          CW_COADate = GetCertificateDate( entities, _srdx.Number, warnings );
        }
        else if ( _code.Contains( Settings.CustomsProcedureCodeN865 ) || _code.Contains( Settings.CustomsProcedureCodeN954 ) )
        {
          CW_CertificateOfOrgin = _srdx.Number.GetFirstCapture( Settings.GetParameter( entities, SettingsEntry.GoodsDescription_CertificateOfOrgin_Pattern ), _na, _stringsList );
          CW_CODate = GetCertificateDate( entities, _srdx.Number, warnings );
        }
      }
      Convert2Warnings( warnings, _stringsList, false );
      ValidToDate = CalculateValidToDate( entities, CW_COADate, CW_CODate );
    }
    private DateTime CalculateValidToDate( Entities entities, DateTime? CW_COADate, DateTime? CW_CODate )
    {
      double _validPeriod = 365;//in days - 
      DateTime _ret = DateTime.Today;
      if ( !CW_COADate.HasValue || !CW_CODate.HasValue )
        return _ret + TimeSpan.FromDays( _validPeriod );
      _ret = CW_CODate.GetValueOrDefault( DateTime.Today ) < _ret ? CW_CODate.GetValueOrDefault( DateTime.Today ) : _ret;
      _ret = CW_COADate.GetValueOrDefault( DateTime.Today ) < _ret ? CW_COADate.GetValueOrDefault( DateTime.Today ) : _ret;
      if ( !Double.TryParse( Settings.GetParameter( entities, SettingsEntry.DefaultValidToDatePeriod ), out _validPeriod ) )
        _validPeriod = 365; //TODO add warning
      return DateTime.Now.Date + TimeSpan.FromDays( _validPeriod );
    }
    private DateTime? GetCertificateDate( Entities entities, string code, List<Warnning> warnings )
    {
      DateTime? _ret = new Nullable<DateTime>();
      try
      {
        MatchCollection _result = Regex.Matches( code, Settings.GetParameter( entities, SettingsEntry.LooselyFormatedDate ) );
        _ret = new DateTime( int.Parse( _result[ 3 ].Value ), int.Parse( _result[ 2 ].Value ), int.Parse( _result[ 1 ].Value ) );
      }
      catch ( Exception _ex )
      {
        string _mssg = String.Format( "Cannot get data from the certificate {0}, because of the error {1}", code, _ex.Message );
        warnings.Add( new Warnning( _mssg, false ) );
      }
      return _ret;
    }
    private void AnalizeGoodsDescription( Entities edc, string goodsDescription, List<Warnning> warnings )
    {
      List<string> _stringsList = new List<string>();
      CWPackageUnits = Convert2Double( goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CWPackageUnits_Pattern ), _na, _stringsList ), _stringsList );
      CWQuantity = Convert2Double( goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CWQuantity_Pattern ), _na, _stringsList ), _stringsList );
      Units = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_Units_Pattern ), _na, _stringsList );
      Convert2Warnings( warnings, _stringsList, false );
    }
    private static void Convert2Warnings( List<Warnning> warningsList, List<string> stringsList, bool fatal )
    {
      if ( stringsList.Count == 0 )
        return;
      warningsList.AddRange( from _erx in stringsList select new Warnning( _erx, fatal ) );
    }
    string _na = "Not recognized";
    private double? Convert2Double( string vlaue, List<string> errors )
    {
      return new Nullable<double>();
    }
    private class CreateCWAccountException: Exception
    {
      public CreateCWAccountException( string message ) : base( message ) { }
    }
    #endregion

  }
}
