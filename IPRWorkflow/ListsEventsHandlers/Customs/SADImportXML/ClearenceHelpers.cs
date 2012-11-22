using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint.Linq;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Customs
{
  internal static class ClearenceHelpers
  {

    #region MyRegion
    internal static void DeclarationProcessing( SADDocumentType _sad, Entities _edc, CustomsDocument.DocumentType _messageType, out string _comments )
    {
      string _at = "started";
      _comments = "Clearance association error";
      try
      {
        switch ( _messageType )
        {
          case CustomsDocument.DocumentType.SAD:
          case CustomsDocument.DocumentType.PZC:
            SADPZCProcessing( _edc, _messageType, _sad, ref _comments, ref _at );
            break;
          case CustomsDocument.DocumentType.IE529:
            _at = "ReExportOfGoods";
            _comments = "Reexport of goods failed";
            foreach ( SADGood _gdx in _sad.SADGood )
              ClearThroughCustoms( _edc, _gdx );
            _comments = "Reexport of goods";
            break;
          case CustomsDocument.DocumentType.CLNE:
            _at = "FimdClearence";
            foreach ( Clearence _cx in Clearence.GetClearence( _edc, _sad.ReferenceNumber ) )
            {
              _cx.DocumentNo = _sad.DocumentNumber;
              _at = "switch RequestedProcedure";
              switch ( _cx.ProcedureCode.RequestedProcedure() )
              {
                case CustomsProcedureCodes.FreeCirculation:
                  _cx.ClearThroughCustoms( _edc );
                  break;
                case CustomsProcedureCodes.InwardProcessing:
                  CreateIPRAccount( _edc, _cx, CustomsDocument.DocumentType.SAD, _sad.CustomsDebtDate.Value, out _comments );
                  break;
                case CustomsProcedureCodes.ReExport:
                case CustomsProcedureCodes.NoProcedure:
                case CustomsProcedureCodes.CustomsWarehousingProcedure:
                default:
                  throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected procedure code for CLNE message", null, _wrongProcedure );
              }
            }
            break;
          default:
            throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected message type.", null, "Unexpected message type." );
        }//switch (_documentType
      }
      catch ( InputDataValidationException _idce )
      {
        throw _idce;
      }
      catch ( IPRDataConsistencyException _iorex )
      {
        throw _iorex;
      }
      catch ( GenericStateMachineEngine.ActionResult _ar )
      {
        throw _ar;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "Clearence analyses error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
    }
    #endregion

    #region private
    private static void SADPZCProcessing( Entities _edc, CustomsDocument.DocumentType _messageType, SADDocumentType _sad, ref string _comments, ref string _at )
    {
      _at = "_customsProcedureCodes";
      foreach ( SADGood _sgx in _sad.SADGood )
      {
        CustomsProcedureCodes _customsProcedureCodes = _sgx.Procedure.RequestedProcedure();
        switch ( _customsProcedureCodes )
        {
          case CustomsProcedureCodes.FreeCirculation:
            _at = "FimdClearence";
            if ( _messageType == CustomsDocument.DocumentType.PZC )
              ClearThroughCustoms( _edc, _sgx );
            else
              _comments = "Document added";
            break;
          case CustomsProcedureCodes.InwardProcessing:
            {
              _at = "NewClearence";
              Clearence _newClearance = Clearence.CreataClearence( _edc, "InwardProcessing", ClearenceProcedure._5171, _sgx );
              if ( _messageType == CustomsDocument.DocumentType.PZC )
              {
                _at = "CreateIPRAccount";
                CreateIPRAccount( _edc, _newClearance, CustomsDocument.DocumentType.PZC, _sad.CustomsDebtDate.Value, out _comments );
              }
              else
                _comments = "Document added";
              break;
            }
          case CustomsProcedureCodes.CustomsWarehousingProcedure:
            _at = "NewClearence";
            Clearence _newWarehousinClearance = new Clearence()
            {
              DocumentNo = _sad.DocumentNumber,
              ReferenceNumber = _sad.ReferenceNumber,
              SADConsignmentLibraryIndex = null,
              ProcedureCode = "CustomsWarehousingProcedure",
              Status = false,
              //[pr4-3738] CustomsProcedureCodes.CustomsWarehousingProcedure 7100 must be added http://itrserver/Bugs/BugDetail.aspx?bid=3738
              ClearenceProcedure = ClearenceProcedure._7100,
            };
            _at = "InsertOnSubmit";
            _edc.Clearence.InsertOnSubmit( _newWarehousinClearance );
            if ( _messageType == CustomsDocument.DocumentType.PZC )
              CreateCWAccount( _edc, _sad, _sgx );// TODO CreateStockRecord  
            else
              _comments = "Document added";
            break;
          case CustomsProcedureCodes.NoProcedure:
          case CustomsProcedureCodes.ReExport:
          default:
            throw new IPRDataConsistencyException( "Clearence.Associate", string.Format( "Unexpected procedure code for the {0} message", _messageType ), null, _wrongProcedure );

        }
      } //switch (_customsProcedureCodes)
    }
    private static void CreateCWAccount( Entities _edc, SADDocumentType _sad, SADGood _sgx )
    {
      //TODO CreateCWAccountNotImplementedException 
      throw new NotImplementedException();
    }
    /// <summary>
    /// Creates the IPR account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="_messageType">Type of the _message.</param>
    /// <param name="customsDebtDate">The customs debt date.</param>
    /// <param name="_comments">The _comments.</param>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    /// <exception cref="InputDataValidationException"></exception>
    private static void CreateIPRAccount
      ( Entities entities, Clearence clearence, CustomsDocument.DocumentType _messageType, DateTime customsDebtDate, out string _comments )
    {
      string _at = "started";
      _comments = "IPR account creation error";
      SADDocumentType declaration = clearence.Clearence2SadGoodID.SADDocumentIndex;
      try
      {
        if ( WebsiteModel.Linq.IPR.RecordExist( entities, clearence.DocumentNo ) )
        {
          string _msg = "IPR record with the same SAD document number: {0} exist";
          throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _msg, clearence.DocumentNo ) );
        }
        _at = "newIPRData";
        _comments = "Inconsistent or incomplete data to create IPR account";
        IPRData _iprdata = new IPRData( entities, clearence.Clearence2SadGoodID, _messageType );
        List<string> _ar = new List<string>();
        if ( !_iprdata.Validate( entities, _ar ) )
          throw new InputDataValidationException( "Inconsistent or incomplete data to create IPR account", "Create IPR Account", _ar );
        _at = "Consent.Lookup";
        _comments = "Consent lookup filed";
        _at = "PCNCode.AddOrGet";
        _comments = "PCN lookup filed";
        PCNCode _pcn = PCNCode.AddOrGet( entities, _iprdata.PCNTariffCode, _iprdata.TobaccoName );
        _at = "new IPRClass";
        IPRClass _ipr = new IPRClass()
        {
          AccountClosed = false,
          AccountBalance = _iprdata.GrossMass,
          Batch = _iprdata.Batch,
          Cartons = _iprdata.Cartons,
          ClearenceIndex = clearence,
          ClosingDate = CAS.SharePoint.Extensions.SPMinimum,
          IPR2ConsentTitle = _iprdata.ConsentLookup,
          Currency = declaration.Currency,
          CustomsDebtDate = customsDebtDate,
          DocumentNo = clearence.DocumentNo,
          Duty = _iprdata.Duty,
          DutyName = _iprdata.DutyName,
          IPRDutyPerUnit = _iprdata.DutyPerUnit,
          Grade = _iprdata.GradeName,
          GrossMass = _iprdata.GrossMass,
          InvoiceNo = _iprdata.Invoice,
          IPRLibraryIndex = declaration.SADDocumenLibrarytIndex,
          NetMass = _iprdata.NetMass,
          OGLValidTo = customsDebtDate + _iprdata.ConsentPeriodCalculated,
          IPR2PCNPCN = _pcn,
          SKU = _iprdata.SKU,
          TobaccoName = _iprdata.TobaccoName,
          TobaccoNotAllocated = _iprdata.GrossMass,
          Title = "-- creating -- ",
          IPRUnitPrice = _iprdata.UnitPrice,
          Value = _iprdata.Value,
          VATName = _iprdata.VATName,
          VAT = _iprdata.VAT,
          IPRVATPerUnit = _iprdata.VATPerUnit
        };
        _at = "new InsertOnSubmit";
        entities.IPR.InsertOnSubmit( _ipr );
        _at = "new SubmitChanges #1";
        entities.SubmitChanges();
        _ipr.Title = String.Format( "IPR-{0:D4}{1:D6}", DateTime.Today.Year, _ipr.Identyfikator );
        if ( _iprdata.Cartons > 0 )
          _ipr.AddDisposal( entities, Convert.ToDecimal( _iprdata.Cartons ) );
        _at = "new SubmitChanges #2";
        entities.SubmitChanges();
      }
      catch ( InputDataValidationException _idve )
      {
        throw _idve;
      }
      catch ( GenericStateMachineEngine.ActionResult _ex )
      {
        _ex.Message.Insert( 0, String.Format( "Message={0}, Reference={1}; ", _messageType, declaration.ReferenceNumber ) );
        throw _ex;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "CreateIPRAccount method error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, "IPR account creation error" );
      }
      _comments = "IPR account created";
    }
    private class IPRData
    {
      #region private
      private void AnalizeGood( SADGood good, CustomsDocument.DocumentType _messageType )
      {
        string _at = "Started";
        try
        {
          _at = "GrossMass";
          SADDocumentType _document = good.SADDocumentIndex;
          if ( _messageType == CustomsDocument.DocumentType.SAD )
            GrossMass = good.GrossMass.HasValue ? good.GrossMass.Value : _document.GrossMass.Value;
          else if ( _messageType == CustomsDocument.DocumentType.PZC )
            GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : good.GrossMass.Value;
          else
            throw new IPRDataConsistencyException( "IPRData.GetCartons", String.Format( "Unexpected message {0} type", _messageType ), null, "Unexpected message" );
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
      #endregion

      #region cretor
      /// <summary>
      /// Initializes a new instance of the <see cref="IPRData" /> class.
      /// </summary>
      /// <param name="good">The good.</param>
      /// <param name="_messageType">Type of the _message.</param>
      /// <exception cref="IPRDataConsistencyException">There is not attached any consent document with code = 1PG1/C601</exception>
      /// <exception cref="InputDataValidationException">Syntax errors in the good description.</exception>
      internal IPRData( Entities edc, SADGood good, CustomsDocument.DocumentType _messageType )
      {
        string _at = "starting";
        try
        {
          PCNTariffCode = good.PCNTariffCode;
          AnalizeGood( good, _messageType );
          AnalizeDutyAndVAT( good );
          _at = "InvoiceNo";
          this.Invoice = ( from _dx in good.SADRequiredDocuments
                           let CustomsProcedureCode = _dx.Code.ToUpper()
                           where CustomsProcedureCode.Contains( "N380" ) || CustomsProcedureCode.Contains( "N935" )
                           select new { Number = _dx.Number }
                          ).First().Number;
          _at = "Consent";
          FindConsentRecord( edc, good.SADRequiredDocuments );
          AnalizeGoodsDescription( edc, good.GoodsDescription );
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
          this.ConsentLookup = IPR.WebsiteModel.Linq.Consent.Find( edc, _nr );
          if ( this.ConsentLookup == null )
          {
            m_Warnings.Add( "Cannot find consent document with number: " + _nr + ". The Consent period is 90 days");
            ConsentPeriodCalculated = TimeSpan.FromDays( 90 );
          }
          else
            ConsentPeriodCalculated = TimeSpan.FromDays( Convert.ToInt32( this.ConsentLookup.ConsentPeriod.Value ) * 30 );
        }
      }
      #endregion

      #region public
      //TODO
      internal bool Validate( Entities entities, List<string> warnnings )
      {
        bool _ret = m_Warnings.Count > 0;
        warnnings.AddRange( m_Warnings );
        return _ret;
      }
      internal double Cartons { get; private set; }
      internal Consent ConsentLookup { get; private set; }
      internal TimeSpan ConsentPeriodCalculated { get; private set; }
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
      internal string PCNTariffCode { get; private set; }
      internal string SKU { get; private set; }
      #endregion

    }
    private static void ClearThroughCustoms( Entities entities, SADGood good )
    {
      bool _ifAny = false;
      foreach ( SADRequiredDocuments _rdx in good.SADRequiredDocuments )
      {
        if ( _rdx.Code != XMLResources.RequiredDocumentConsignmentCode )
          continue;
        int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( _rdx.Number, Settings.GetParameter( entities, SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern ) );
        if ( _cleranceInt.HasValue )
        {
          Clearence _clearance = Element.GetAtIndex<Clearence>( entities.Clearence, _cleranceInt.Value );
          _clearance.ClearThroughCustoms( entities, good );
          _ifAny = true;
        }
      }// foreach 
      if ( !_ifAny )
      {
        string _template = "Cannot find required document code ={0} for customs document = {1}/ref={2}";
        throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _template, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber ) );
      }
    }
    /// <summary>
    /// Get requested customs procedure code 
    /// </summary>
    /// <param name="_cpc">The Customs Procedure Code.</param>
    /// <returns>Requested procedure code <see cref="CustomsProcedureCodes"/> - first two chars of the box 37</returns>
    private static CustomsProcedureCodes RequestedProcedure( this string _cpc )
    {
      switch ( _cpc.Remove( 2 ) )
      {
        case "00":
          return CustomsProcedureCodes.NoProcedure;
        case "31":
          return CustomsProcedureCodes.ReExport;
        case "40":
          return CustomsProcedureCodes.FreeCirculation;
        case "51":
          return CustomsProcedureCodes.InwardProcessing;
        case "71":
          return CustomsProcedureCodes.CustomsWarehousingProcedure;
        default:
          throw new CustomsDataException( "Extensions.RequestedProcedure", "Unsupported requested procedure" );
      }
    }
    private const string _wrongProcedure = "Wrong customs procedure";
    #endregion

  }
}
