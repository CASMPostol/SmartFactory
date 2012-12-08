using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Account;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint.Linq;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Customs
{
  internal static class ClearenceHelpers
  {

    #region public
    internal static void DeclarationProcessing( SADDocumentType _sad, Entities _edc, CustomsDocument.DocumentType _messageType, out string _comments, List<InputDataValidationException> warnings )
    {
      string _at = "started";
      _comments = "Clearance association error";
      try
      {
        switch ( _messageType )
        {
          case CustomsDocument.DocumentType.SAD:
          case CustomsDocument.DocumentType.PZC:
            SADPZCProcessing( _edc, _messageType, _sad, ref _comments, ref _at, warnings );
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
            IQueryable<Clearence> _clrncs = Clearence.GetClearence( _edc, _sad.ReferenceNumber );
            if ( ( from _cx in _clrncs where _cx.Clearence2SadGoodID == null select _cx ).Any<Clearence>() )
            {
              string _error = String.Format( "SAD with reference number: {0} must be imported first", _sad.ReferenceNumber );
              throw new InputDataValidationException( "CLNE message cannot be processed befor SAD", "DeclarationProcessing", _error );
            }
            foreach ( Clearence _cx in _clrncs )
            {
              _cx.DocumentNo = _sad.DocumentNumber;
              _at = "switch RequestedProcedure";
              switch ( _cx.ProcedureCode.RequestedProcedure() )
              {
                case CustomsProcedureCodes.FreeCirculation:
                  _cx.FinishClearingThroughCustoms( _edc );
                  break;
                case CustomsProcedureCodes.InwardProcessing:
                  CreateIPRAccount( _edc, _cx, CustomsDocument.DocumentType.SAD, _sad.CustomsDebtDate.Value, out _comments, warnings );
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
    private static void SADPZCProcessing( Entities edc, CustomsDocument.DocumentType messageType, SADDocumentType sad, ref string comments, ref string at, List<InputDataValidationException> warnings )
    {
      at = "_customsProcedureCodes";
      foreach ( SADGood _sgx in sad.SADGood )
      {
        CustomsProcedureCodes _customsProcedureCodes = _sgx.Procedure.RequestedProcedure();
        switch ( _customsProcedureCodes )
        {
          case CustomsProcedureCodes.FreeCirculation:
            at = "FimdClearence";
            if ( messageType == CustomsDocument.DocumentType.PZC )
              ClearThroughCustoms( edc, _sgx );
            else
              comments = "Document added";
            break;
          case CustomsProcedureCodes.InwardProcessing:
            {
              at = "NewClearence";
              Clearence _newClearance = Clearence.CreataClearence( edc, "InwardProcessing", ClearenceProcedure._5171, _sgx );
              if ( messageType == CustomsDocument.DocumentType.PZC )
              {
                at = "CreateIPRAccount";
                CreateIPRAccount( edc, _newClearance, CustomsDocument.DocumentType.PZC, sad.CustomsDebtDate.Value, out comments, warnings );
              }
              else
                comments = "Document added";
              break;
            }
          case CustomsProcedureCodes.CustomsWarehousingProcedure:
            at = "NewClearence";
            Clearence _newWarehousinClearance = Clearence.CreataClearence( edc, "CustomsWarehousingProcedure", ClearenceProcedure._7100, _sgx );
            if ( messageType == CustomsDocument.DocumentType.PZC )
              CreateCWAccount( edc, sad, _sgx );
            else
              comments = "Document added";
            break;
          case CustomsProcedureCodes.NoProcedure:
          case CustomsProcedureCodes.ReExport:
          default:
            throw new IPRDataConsistencyException( "Clearence.Associate", string.Format( "Unexpected procedure code for the {0} message", messageType ), null, _wrongProcedure );
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
    /// <param name="warnings">The warnings.</param>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    /// <exception cref="InputDataValidationException"></exception>
    private static void CreateIPRAccount
      ( Entities entities, Clearence clearence, CustomsDocument.DocumentType _messageType, DateTime customsDebtDate, out string _comments, List<InputDataValidationException> warnings )
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
        IPRAccountData _iprdata = new IPRAccountData( entities, clearence.Clearence2SadGoodID, Convert2MessageType( _messageType ) );
        List<string> _ar = new List<string>();
        if ( !_iprdata.Validate( entities, _ar ) )
          warnings.Add( new InputDataValidationException( "Inconsistent or incomplete data to create IPR account", "Create IPR Account", _ar ) );
        _at = "Consent.Lookup";
        _comments = "Consent lookup filed";
        _at = "new IPRClass";
        IPRClass _ipr = new IPRClass( entities, _iprdata, clearence, declaration, customsDebtDate );
        _at = "new InsertOnSubmit";
        entities.IPR.InsertOnSubmit( _ipr );
        clearence.Status = true;
        _at = "new SubmitChanges #1";
        entities.SubmitChanges();
        _ipr.UpdateTitle();
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
    private static AccountData.MessageType Convert2MessageType( CustomsDocument.DocumentType type )
    {
      AccountData.MessageType _ret = default( AccountData.MessageType );
      switch ( type )
      {
        case CustomsDocument.DocumentType.SAD:
          _ret = AccountData.MessageType.SAD;
          break;
        case CustomsDocument.DocumentType.PZC:
          _ret = AccountData.MessageType.SAD;
          break;
        case CustomsDocument.DocumentType.IE529:
        case CustomsDocument.DocumentType.CLNE:
        default:
          throw new ArgumentException( "Out of range value for CustomsDocument.DocumentType argument in Convert2MessageType ", "type" );
      }
      return _ret;
    }
    private static void ClearThroughCustoms( Entities entities, SADGood good )
    {
      bool _ifAny = false;
      foreach ( SADRequiredDocuments _rdx in good.SADRequiredDocuments )
      {
        if ( _rdx.Code != XMLResources.RequiredDocumentConsignmentCode )
          continue;
        int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( _rdx.Number, Settings.GetParameter( entities, SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern ) );
        if ( !_cleranceInt.HasValue )
          continue;
        Clearence _clearance = Element.GetAtIndex<Clearence>( entities.Clearence, _cleranceInt.Value );
        _clearance.FinishClearingThroughCustoms( entities, good );
        _ifAny = true;
      }// foreach 
      if ( !_ifAny )
      {
        string _template = "Cannot find required document code={0} for customs document = {1}/ref={2}";
        throw new InputDataValidationException(
          String.Format( _template, XMLResources.RequiredDocumentConsignmentCode, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber ),
          "SAD Required Documents",
          "clear through castoms fatal error" );
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
