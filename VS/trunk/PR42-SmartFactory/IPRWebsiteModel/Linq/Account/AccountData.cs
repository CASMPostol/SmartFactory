using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Account Data
  /// </summary>
  public abstract class AccountData
  {
    #region private
    private void AnalizeGood( SADGood good, MessageType _messageType )
    {
      string _at = "Started";
      try
      {
        _at = "GrossMass";
        SADDocumentType _document = good.SADDocumentIndex;
        switch ( _messageType )
        {
          case MessageType.PZC:
            GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : good.GrossMass.Value;
            break;
          case MessageType.SAD:
            GrossMass = good.GrossMass.HasValue ? good.GrossMass.Value : _document.GrossMass.Value;
            break;
        }
        _at = "SADQuantity";
        SADQuantity _quantity = good.SADQuantity.FirstOrDefault();
        NetMass = _quantity == null ? 0 : _quantity.NetMass.GetValueOrDefault( 0 );
        _at = "Cartons";
        SADPackage _packagex = good.SADPackage.First();
        if ( _packagex.Package.ToUpper().Contains( "CT" ) )
          Cartons = GrossMass - NetMass;
        else
          Cartons = 0;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "AnalizeGood error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    private void AnalizeDutyAndVAT( SADGood good )
    {
      string _at = "Started";
      try
      {
        Duty = 0;
        VAT = 0;
        DutyName = string.Empty;
        VATName = string.Empty;
        foreach ( SADDuties _duty in good.SADDuties )
        {
          _at = "switch " + _duty.DutyType;
          switch ( _duty.DutyType )
          {
            //Duty
            case "A10":
            case "A00":
            case "A20":
              Duty += _duty.Amount.Value;
              _at = "DutyName";
              DutyName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
              break;
            //VAT
            case "B00":
            case "B10":
            case "B20":
              VAT += _duty.Amount.Value;
              _at = "VATName";
              VATName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
              break;
            default:
              break;
          }
        }
        _at = "DutyPerUnit";
        DutyPerUnit = Duty / NetMass;
        _at = "VATPerUnit";
        VATPerUnit = VAT / NetMass;
        _at = "Value";
        Value = good.TotalAmountInvoiced.Value;
        _at = "UnitPrice";
        UnitPrice = Value / NetMass;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "AnalizeDutyAndVAT error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    private const string UnrecognizedName = "-- unrecognized name --";
    /// <summary>
    /// Analizes the goods description.
    /// </summary>
    /// <param name="_GoodsDescription">The _ goods description.</param>
    /// <exception cref="CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException">Syntax errors in the good description.</exception>
    private void AnalizeGoodsDescription( Entities edc, string _GoodsDescription )
    {
      List<string> _sErrors = new List<string>();
      string _na = "Not recognized";
      TobaccoName = _GoodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionTobaccoNamePattern ), _na, _sErrors );
      GradeName = _GoodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionWGRADEPattern ), _na, _sErrors );
      SKU = _GoodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionSKUPattern ), _na, _sErrors );
      Batch = _GoodsDescription.GetFirstCapture( Settings.GetParameter( edc, SettingsEntry.GoodsDescriptionBatchPattern ), _na, _sErrors );
      if ( _sErrors.Count > 0 )
        throw new InputDataValidationException( "Syntax errors in the good description.", "AnalizeGoodsDescription", _sErrors );
    }
    private List<string> m_Warnings = new List<string>();
    private void FindConsentRecord( Entities edc, EntitySet<SADRequiredDocuments> sadRequiredDocumentsEntitySet )
    {
      SADRequiredDocuments _rd = ( from _dx in sadRequiredDocumentsEntitySet
                                   let CustomsProcedureCode = _dx.Code.ToUpper()
                                   where CustomsProcedureCode.Contains( "1PG1" ) || CustomsProcedureCode.Contains( "C601" )
                                   select _dx
                                  ).FirstOrDefault();
      if ( _rd == null )
        m_Warnings.Add( "There is not attached any consent document with code = 1PG1/C601" );
      else
      {
        string _nr = _rd.Number.Trim();
        this.ConsentLookup = Consent.Find( edc, _nr );
        if ( this.ConsentLookup == null )
        {
          m_Warnings.Add( "Cannot find consent document with number: " + _nr + ". The Consent period is 90 days" );
          this.ConsentLookup = new Consent()
          {
            ConsentDate = CAS.SharePoint.Extensions.DateTimeNull,
            ConsentPeriod = TimeSpan.FromDays( 90 ).TotalDays,
            IsIPR = true,
            ValidFromDate = CAS.SharePoint.Extensions.DateTimeNull,
            ValidToDate = CAS.SharePoint.Extensions.DateTimeNull
          };
        }
      }
    }
    #endregion

    #region cretor

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountData" /> class.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="good">The good.</param>
    /// <param name="_messageType">Type of the _message.</param>
    /// <exception cref="IPRDataConsistencyException">There is not attached any consent document with code = 1PG1/C601</exception>
    /// <exception cref="InputDataValidationException">Syntax errors in the good description.</exception>
    protected AccountData( Entities edc, SADGood good, MessageType _messageType )
    {
      string _at = "starting";
      try
      {
        AnalizeGood( good, _messageType );
        AnalizeDutyAndVAT( good );
        _at = "Invoice";
        this.Invoice = ( from _dx in good.SADRequiredDocuments
                         let CustomsProcedureCode = _dx.Code.ToUpper()
                         where CustomsProcedureCode.Contains( "N380" ) || CustomsProcedureCode.Contains( "N935" )
                         select new { Number = _dx.Number }
                        ).First().Number;
        _at = "FindConsentRecord";
        FindConsentRecord( edc, good.SADRequiredDocuments );
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
    public double Cartons { get; private set; }
    internal Consent ConsentLookup { get; private set; }
    internal double Duty { get; private set; }
    internal string DutyName { get; private set; }
    internal double DutyPerUnit { get; private set; }
    internal string GradeName { get; private set; }
    internal double GrossMass { get; private set; }
    internal string Invoice { get; private set; }
    internal double NetMass { get; private set; }
    internal string TobaccoName { get; private set; }
    internal double UnitPrice { get; private set; }
    internal double Value { get; private set; }
    internal string VATName { get; private set; }
    internal double VAT { get; private set; }
    internal double VATPerUnit { get; private set; }
    internal string Batch { get; private set; }
    internal PCNCode PCNTariffCode { get; private set; }
    internal string SKU { get; private set; }
    #endregion

  }

}
