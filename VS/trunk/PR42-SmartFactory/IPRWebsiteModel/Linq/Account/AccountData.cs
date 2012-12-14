﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Account Data
  /// </summary>
  public abstract class AccountData
  {

    #region cretor
    /// <summary>
    /// Initializes a new instance of the <see cref="AccountData" /> class.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> object.</param>
    /// <param name="good">The good.</param>
    /// <param name="messageType">Type of the customs message.</param>
    /// <exception cref="IPRDataConsistencyException">There is not attached any consent document with code = 1PG1/C601</exception>
    /// <exception cref="InputDataValidationException">Syntax errors in the good description.</exception>
    protected AccountData( Entities edc, SADGood good, MessageType messageType )
    {
      string _at = "starting";
      try
      {
        DateTime _customsDebtDate = good.SADDocumentIndex.CustomsDebtDate.Value;
        this.CustomsDebtDate = _customsDebtDate;
        AnalizeGood( good, messageType );
        _at = "Value";
        Value = good.TotalAmountInvoiced.GetValueOrDefault( 0 );
        _at = "UnitPrice";
        UnitPrice = Value / NetMass;
        _at = "Invoice";
        this.Invoice = ( from _dx in good.SADRequiredDocuments
                         let CustomsProcedureCode = _dx.Code.ToUpper()
                         where CustomsProcedureCode.Contains( "N380" ) || CustomsProcedureCode.Contains( "N935" )
                         select new { Number = _dx.Number }
                        ).First().Number;
        _at = "FindConsentRecord";
        FindConsentRecord( edc, good.SADRequiredDocuments, _customsDebtDate );
        _at = "AnalizeGoodsDescription";
        AnalizeGoodsDescription( edc, good.GoodsDescription );
        _at = "PCN lookup filed";
        PCNTariffCode = PCNCode.AddOrGet( edc, good.PCNTariffCode, TobaccoName );
      }
      catch ( InputDataValidationException _idve )
      {
        throw _idve;
      }
      catch ( IPRDataConsistencyException es )
      {
        throw es;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "IPR.IPRData creator error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    #endregion

    #region private
    /// <summary>
    /// Analizes the good.
    /// </summary>
    /// <param name="good">The good.</param>
    /// <param name="messageType">Type of the _message.</param>
    /// <exception cref="IPRDataConsistencyException"></exception>
    protected internal virtual void AnalizeGood( SADGood good, MessageType messageType )
    {
      string _at = "Started";
      try
      {
        _at = "GrossMass";
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
        _at = "SADQuantity";
        GetNetMass( good );
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "AnalizeGood error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="good">The good.</param>
    protected internal abstract void GetNetMass( SADGood good );
    private const string UnrecognizedName = "-- unrecognized name --";
    /// <summary>
    /// Analizes the goods description.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="goodsDescription">The _ goods description.</param>
    /// <exception cref="InputDataValidationException">Syntax errors in the good description.;AnalizeGoodsDescription</exception>
    /// <exception cref="CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException">Syntax errors in the good description.</exception>
    protected virtual void AnalizeGoodsDescription( Entities edc, string goodsDescription )
    {
      ErrorsList _sErrors = new ErrorsList();
      string _na = "Not recognized";
      TobaccoName = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionTobaccoNamePattern ), _na, _sErrors );
      GradeName = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionWGRADEPattern ), _na, _sErrors );
      SKU = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionSKUPattern ), _na, _sErrors );
      Batch = goodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionBatchPattern ), _na, _sErrors );
      if ( _sErrors.Count > 0 )
        throw new InputDataValidationException( "Syntax errors in the good description.", "AnalizeGoodsDescription", _sErrors );
    }
    private List<string> m_Warnings = new List<string>();
    private void FindConsentRecord( Entities edc, EntitySet<SADRequiredDocuments> sadRequiredDocumentsEntitySet, DateTime customsDebtDate )
    {
      SADRequiredDocuments _rd = ( from _dx in sadRequiredDocumentsEntitySet
                                   let CustomsProcedureCode = _dx.Code.ToUpper()
                                   where CustomsProcedureCode.Contains( "1PG1" ) || CustomsProcedureCode.Contains( "C601" )
                                   select _dx
                                  ).FirstOrDefault();
      if ( _rd == null )
      {
        m_Warnings.Add( "There is not attached any consent document with code = 1PG1/C601" );
        CreateDefaultConsent( edc, String.Empty.NotAvailable() );
      }
      else
      {
        string _nr = _rd.Number.Trim();
        this.ConsentLookup = Consent.Find( edc, _nr );
        if ( this.ConsentLookup == null )
          CreateDefaultConsent( edc, _nr );
      }
      ValidToDate = customsDebtDate + TimeSpan.FromDays( ConsentLookup.ConsentPeriod.Value ); //TODO different for CW !!!
    }

    private void CreateDefaultConsent( Entities edc, string _nr )
    {
      this.ConsentLookup = Consent.DefaultConsent( edc, Process, _nr );
      string _msg = "Cannot find consent document with number: {0}. The Consent period is {1} months";
      m_Warnings.Add( String.Format( _msg, _nr, this.ConsentLookup.ConsentPeriod ) );
    }
    /// <summary>
    /// Gets the process.
    /// </summary>
    /// <value>
    /// The process.
    /// </value>
    protected internal abstract Consent.CustomsProcess Process { get; }
    #endregion

    #region public
    /// <summary>
    /// Message Type
    /// </summary>
    public enum MessageType
    {
      /// <summary>
      /// The PZC
      /// </summary>
      PZC,
      /// <summary>
      /// The SAD
      /// </summary>
      SAD
    }
    /// <summary>
    /// Validates the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="warnnings">The warnnings.</param>
    /// <returns></returns>
    public bool Validate( Entities entities, List<string> warnnings )
    {
      bool _ret = m_Warnings.Count == 0;
      warnnings.AddRange( m_Warnings );
      return _ret;
    }
    internal Consent ConsentLookup { get; private set; }
    internal DateTime CustomsDebtDate { get; private set; }
    internal string GradeName { get; private set; }
    internal double GrossMass { get; private set; }
    internal string Invoice { get; private set; }
    internal double NetMass { get; set; }
    internal string TobaccoName { get; private set; }
    internal double UnitPrice { get; private set; }
    internal double Value { get; private set; }
    internal string Batch { get; private set; }
    internal PCNCode PCNTariffCode { get; private set; }
    internal string SKU { get; private set; }
    internal DateTime ValidToDate { get; private set; }
    #endregion

  }
}
