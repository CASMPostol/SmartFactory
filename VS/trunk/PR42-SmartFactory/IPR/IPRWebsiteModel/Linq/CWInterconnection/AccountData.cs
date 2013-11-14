//<summary>
//  Title   : Account Data abstract common part
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
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection
{
  /// <summary>
  /// Account Data abstract common part
  /// </summary>
  public abstract class AccountData: CommonAccountData
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountData" /> class.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> object.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="messageType">Type of the customs message.</param>
    /// <param name="ProgressChange">The progress change.</param>
    public virtual void GetAccountData( Entities edc, Clearence clearence, MessageType messageType, ProgressChangedEventHandler ProgressChange )
    {
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.GetAccountData.starting" ) );
      this.ClearenceLookup = clearence.Id.Value;
      DocumentNo = clearence.DocumentNo;
      DateTime _customsDebtDate = clearence.Clearence2SadGoodID.SADDocumentIndex.CustomsDebtDate.Value;
      this.CustomsDebtDate = _customsDebtDate;
      AnalizeGood( clearence.Clearence2SadGoodID, messageType );
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.GetAccountData.Invoice" ) );
      this.Invoice = ( from _dx in clearence.Clearence2SadGoodID.SADRequiredDocuments
                       let CustomsProcedureCode = _dx.Code.ToUpper()
                       where CustomsProcedureCode.Contains( "N380" ) || CustomsProcedureCode.Contains( "N935" )
                       select new { Number = _dx.Number }
                      ).First().Number;
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.GetAccountData.FindConsentRecord" ) );
      FindConsentRecord( edc, clearence.Clearence2SadGoodID.SADRequiredDocuments, _customsDebtDate, ProgressChange );
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.GetAccountData.AnalizeGoodsDescription" ) );
      AnalizeGoodsDescription( edc, clearence.Clearence2SadGoodID.GoodsDescription ); //TODO to IPR
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.GetAccountData.PCNTariffCodeLookup" ) );
      PCNTariffCodeLookup = PCNCode.AddOrGet( edc, clearence.Clearence2SadGoodID.PCNTariffCode, TobaccoName ).Id.Value;
    }
    /// <summary>
    /// Calls the remote service.
    /// </summary>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="warnningList">The warnning list.</param>
    public abstract void CallService( string requestUrl, List<Warnning> warnningList );
    #endregion

    #region private
    /// <summary>
    /// Sets the valid to date.
    /// </summary>
    /// <param name="customsDebtDate">The customs debt date.</param>
    /// <param name="consent">The consent.</param>
    protected internal virtual void SetValidToDate( DateTime customsDebtDate, Consent consent ) { }
    /// <summary>
    /// Analizes the good.
    /// </summary>
    /// <param name="good">The good.</param>
    /// <param name="messageType">Type of the _message.</param>
    protected internal virtual void AnalizeGood( SADGood good, MessageType messageType )
    {
      SADDocumentType _document = good.SADDocumentIndex;
      switch ( messageType )
      {
        case MessageType.PZC:
          GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : good.GrossMass.Value;
          break;
        case MessageType.SAD:
          GrossMass = good.GrossMass.HasValue ? good.GrossMass.Value : _document.GrossMass.Value;
          break;
      }
      GetNetMass( good );
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="good">The good.</param>
    protected internal abstract void GetNetMass( SADGood good );
    /// <summary>
    /// Analizes the goods description.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="goodsDescription">The _ goods description.</param>
    /// <exception cref="InputDataValidationException">Syntax errors in the good description.;AnalizeGoodsDescription</exception>
    /// <exception cref="CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException">Syntax errors in the good description.</exception>
    private void AnalizeGoodsDescription( Entities edc, string goodsDescription )
    {
      List<string> _sErrors = new List<string>();
      string _na = "Not recognized";
      TobaccoName = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionTobaccoNamePattern ), _na, _sErrors );
      GradeName = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionWGRADEPattern ), _na, _sErrors );
      SKU = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionSKUPattern ), _na, _sErrors );
      this.BatchId = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionBatchPattern ), _na, _sErrors );
      if ( _sErrors.Count > 0 )
      {
        ErrorsList _el = new ErrorsList();
        _el.Add( _sErrors, true );
        throw new InputDataValidationException( "Syntax errors in the good description.", "AnalizeGoodsDescription", _el );
      }
    }
    private void FindConsentRecord( Entities edc, EntitySet<SADRequiredDocuments> sadRequiredDocumentsEntitySet, DateTime customsDebtDate, ProgressChangedEventHandler ProgressChange )
    {
      ProgressChange( this, new ProgressChangedEventArgs( 1, "AccountData.FindConsentRecord" ) );
      SADRequiredDocuments _rd = ( from _dx in sadRequiredDocumentsEntitySet
                                   let CustomsProcedureCode = _dx.Code.ToUpper()
                                   where CustomsProcedureCode.Contains( GlobalDefinitions.CustomsProcedureCodeC600 ) ||
                                         CustomsProcedureCode.Contains( GlobalDefinitions.CustomsProcedureCodeC601 ) ||
                                         CustomsProcedureCode.Contains( GlobalDefinitions.CustomsProcedureCode1PG1 )
                                   select _dx
                                  ).FirstOrDefault();
      Linq.Consent _cnst = null;
      if ( _rd == null )
      {
        ProgressChange( this, new ProgressChangedEventArgs( 1, new Customs.Warnning( "There is not attached any consent document with code = C600/C601/1PG1", false ) ) );
        _cnst = CreateDefaultConsent( edc, String.Empty.NotAvailable(), ProgressChange );
      }
      else
      {
        string _nr = _rd.Number.Trim();
        _cnst = Consent.Find( edc, _nr );
        if ( _cnst == null )
          _cnst = CreateDefaultConsent( edc, _nr, ProgressChange );
        this.ConsentLookup = _cnst.Id.Value;
      }
      this.SetValidToDate( customsDebtDate, _cnst );
    }
    private Linq.Consent CreateDefaultConsent( Entities edc, string _nr, ProgressChangedEventHandler ProgressChange )
    {
      Linq.Consent _ret = Consent.DefaultConsent( edc, GetCustomsProcess( Process ), _nr );
      string _msg = "Created default consent document with number: {0}. The Consent period is {1} months";
      ProgressChange( this, new ProgressChangedEventArgs( 1, new Customs.Warnning( String.Format( _msg, _nr, _ret.ConsentPeriod ), false ) ) );
      return _ret;
    }
    private static Consent.CustomsProcess GetCustomsProcess( CustomsProcess process )
    {
      Consent.CustomsProcess _ret = default( Consent.CustomsProcess );
      switch ( process )
      {
        case CustomsProcess.ipr:
          _ret = Consent.CustomsProcess.ipr;
          break;
        case CustomsProcess.cw:
          _ret = Consent.CustomsProcess.cw;
          break;
      }
      return _ret;
    }
    #endregion
  }
}
