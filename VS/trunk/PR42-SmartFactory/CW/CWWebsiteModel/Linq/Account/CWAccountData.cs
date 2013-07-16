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
using System.Linq;
using System.Collections.Generic;
using CAS.SharePoint.Web;
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
    public CWAccountData() { }

    internal CommonAccountData CommonAccountData { get; private set; }
    internal string CW_CertificateOfOrgin { get; private set; }  //TODO from Required documents. 
    internal double? CWMassPerPackage { get; private set; } //TODO Calculated
    internal double? CWPackageKg { get; private set; } // Good description
    internal double? CWPackageUnits { get; private set; } //Good description
    internal string CWPzNo { get; private set; } // Manualy
    internal double? CWQuantity { get; private set; } //Good descriptionc
    internal DateTime? EntryDate { get; private set; } // Today ?
    internal string Units { get; private set; } //Good description
    internal Clearence ClearenceLookup { get; private set; }
    internal Consent ConsentLookup { get; private set; }
    internal string CW_CertificateOfAuthenticity { get; private set; }
    internal DateTime? CW_CODate { get; private set; }
    internal DateTime? CW_COADate { get; private set; }
    public DateTime? ValidToDate { get; private set; }

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
            throw new CreateCWAccount( String.Format( _msg, ClearenceLookup.DocumentNo ) );
          }
          _at = "ProcessCustomsMessage";
          this.CommonAccountData = accountData;
          _at = "GetAtIndex<Clearence>";
          ClearenceLookup = Element.GetAtIndex<Clearence>( _edc.Clearence, accountData.ConsentLookup );
          _at = "GetAtIndex<Consent>";
          Linq.Consent _consentLookup = Element.GetAtIndex<Consent>( _edc.Consent, CommonAccountData.ConsentLookup );
          _at = "AnalizeGoodsDescription";
          AnalizeGoodsDescription( _edc, ClearenceLookup.Clearence2SadGoodID.GoodsDescription, warnings );
          this.CW_CertificateOfOrgin = "TBD";
          this.CWMassPerPackage = 0;
          this.CWPzNo = "M/A";
          this.EntryDate = DateTime.Today.Date;
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
      catch ( CreateCWAccount _ccwe )
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
    private void AnalyzeCertificates( Entities edc, EntitySet<SADRequiredDocuments> sadRequiredDocumentsEntitySet, List<Warnning> warnings )
    {
      //TODO review
      List<string> _stringsList = new List<string>();
      double _validPeriod = 180;
      if ( !Double.TryParse( Settings.GetParameter( edc, SettingsEntry.DefaultValidToDatePeriod ), out _validPeriod ) )
        _validPeriod = 180;
      ValidToDate = DateTime.Now.Date + TimeSpan.FromDays( _validPeriod );
      foreach ( SADRequiredDocuments _srdx in ( from _dx in sadRequiredDocumentsEntitySet select _dx ) )
      {
        string _code = _srdx.Code.Trim().ToUpper();
        if ( _code.Contains( Settings.CustomsProcedureCodeA004 ) )
        {
          CW_CertificateOfAuthenticity = _srdx.Code.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern ), _na, _stringsList );
          CW_COADate = GetCertificateDate( _srdx.Code, warnings );
        }
        else if ( _code.Contains( Settings.CustomsProcedureCodeN865 ) || _code.Contains( Settings.CustomsProcedureCodeN954 ) )
        {
          CW_CertificateOfOrgin = String.Empty;
          CW_CODate = GetCertificateDate( _srdx.Code, warnings );
        }
      }
      Convert2Warning( warnings, _stringsList, false );
    }
    private DateTime? GetCertificateDate( string p, List<Warnning> warnings )
    {
      throw new NotImplementedException();
    }
    private void AnalizeGoodsDescription( Entities edc, string goodsDescription, List<Warnning> warnings )
    {
      List<string> _stringsList = new List<string>();
      CWPackageKg = Convert2Double( goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CWPackageKg_Pattern ), _na, _stringsList ), _stringsList );
      CWPackageUnits = Convert2Double( goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CWPackageKg_Pattern ), _na, _stringsList ), _stringsList );
      CWQuantity = Convert2Double( goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_CWQuantity_Pattern ), _na, _stringsList ), _stringsList );
      Units = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescription_Units_Pattern ), _na, _stringsList );
      Convert2Warning( warnings, _stringsList, false );
    }

    private static void Convert2Warning( List<Warnning> warningsList, List<string> stringsList, bool fatal )
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
    private class CreateCWAccount: Exception
    {
      public CreateCWAccount( string message ) : base( message ) { }
    }
    #endregion

  }
}
