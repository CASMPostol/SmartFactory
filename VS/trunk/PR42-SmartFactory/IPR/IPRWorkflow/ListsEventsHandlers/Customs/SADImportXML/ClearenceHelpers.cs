using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Account;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  internal static class ClearenceHelpers
  {

    #region public
    internal static void DeclarationProcessing( Entities edc, SADDocumentType _sad, CustomsDocument.DocumentType messageType, ref string comments )
    {
      comments = "Clearance association error";
      switch ( messageType )
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          SADPZCProcessing( edc, messageType, _sad, ref comments );
          break;
        case CustomsDocument.DocumentType.IE529:
          IE529Processing( edc, _sad, ref comments );
          break;
        case CustomsDocument.DocumentType.CLNE:
          CLNEProcessing( _sad, edc, ref comments );
          break;
      }//switch (_documentType
    }
    #endregion

    #region private
    private static CustomsProcedureCodes RequestedProcedure( this ClearenceProcedure value )
    {
      CustomsProcedureCodes _ret = default( CustomsProcedureCodes );
      switch ( value )
      {
        case ClearenceProcedure._3151:
        case ClearenceProcedure._3171:
          _ret = CustomsProcedureCodes.NoProcedure;
          break;
        case ClearenceProcedure._4051:
        case ClearenceProcedure._4071:
          _ret = CustomsProcedureCodes.FreeCirculation;
          break;
        case ClearenceProcedure._5100:
        case ClearenceProcedure._5171:
          _ret = CustomsProcedureCodes.InwardProcessing;
          break;
        case ClearenceProcedure._7100:
        case ClearenceProcedure._7171:
          _ret = CustomsProcedureCodes.CustomsWarehousingProcedure;
          break;
      }
      return _ret;
    }
    private static void IE529Processing( Entities _edc, SADDocumentType _sad, ref string _comments )
    {
      _comments = "Reexport of goods failed";
      foreach ( SADGood _gdx in _sad.SADGood )
        ClearThroughCustoms( _edc, _gdx );
      _comments = "Reexport of goods";
    }
    private static void CLNEProcessing( SADDocumentType _sad, Entities _edc, ref string _comments )
    {
      IQueryable<Clearence> _clrncs = Clearence.GetClearence( _edc, _sad.ReferenceNumber );
      if ( ( from _cx in _clrncs where _cx.Clearence2SadGoodID == null select _cx ).Any<Clearence>() )
      {
        string _error = String.Format( "SAD with reference number: {0} must be imported first", _sad.ReferenceNumber );
        throw new InputDataValidationException( "CLNE message cannot be processed befor SAD", "DeclarationProcessing", _error, true );
      }
      foreach ( Clearence _cx in _clrncs )
      {
        _cx.DocumentNo = _sad.DocumentNumber;
        switch ( _cx.ClearenceProcedure.Value.RequestedProcedure() )
        {
          case CustomsProcedureCodes.FreeCirculation:
            _cx.FinishClearingThroughCustoms( _edc );
            break;
          case CustomsProcedureCodes.InwardProcessing:
            CreateIPRAccount( _edc, _cx, CustomsDocument.DocumentType.SAD, out _comments );
            break;
          case CustomsProcedureCodes.CustomsWarehousingProcedure:
            CreateCWAccount( _edc, _cx, CustomsDocument.DocumentType.SAD, out _comments );
            break;
          case CustomsProcedureCodes.ReExport:
          case CustomsProcedureCodes.NoProcedure:
          default:
            throw new IPRDataConsistencyException( "CLNEProcessing", "Unexpected procedure code for CLNE message", null, _wrongProcedure );
        }
      }
    }
    private static void SADPZCProcessing( Entities edc, CustomsDocument.DocumentType messageType, SADDocumentType sad, ref string comments )
    {
      string at = "_customsProcedureCodes";
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
              at = "InwardProcessing:CreataClearence";
              Clearence _newClearance = Clearence.CreataClearence( edc, "InwardProcessing", ClearenceProcedure._5171, _sgx );
              if ( messageType == CustomsDocument.DocumentType.PZC )
              {
                at = "CreateIPRAccount";
                CreateIPRAccount( edc, _newClearance, CustomsDocument.DocumentType.PZC, out comments );
              }
              else
                comments = "Document added";
              break;
            }
          case CustomsProcedureCodes.CustomsWarehousingProcedure:
            at = "CustomsWarehousingProcedure:CreataClearence";
            Clearence _newWarehousinClearance = Clearence.CreataClearence( edc, "CustomsWarehousingProcedure", ClearenceProcedure._7100, _sgx );
            if ( messageType == CustomsDocument.DocumentType.PZC )
            {
              at = "CreateCWAccount";
              CreateCWAccount( edc, _newWarehousinClearance, CustomsDocument.DocumentType.PZC, out comments );
            }
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

    /// <summary>
    /// Creates the IPR account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="messageType">Type of the _message.</param>
    /// <param name="comments">The _comments.</param>
    /// <exception cref="InputDataValidationException">Inconsistent or incomplete data to create IPR account;Create IPR Account</exception>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    private static void CreateIPRAccount( Entities entities, Clearence clearence, CustomsDocument.DocumentType messageType, out string comments )
    {
      string _at = "started";
      comments = "IPR account creation error";
      string _referenceNumber = String.Empty;
      try
      {
        SADDocumentType declaration = clearence.Clearence2SadGoodID.SADDocumentIndex;
        _referenceNumber = declaration.ReferenceNumber;
        if ( WebsiteModel.Linq.IPR.RecordExist( entities, clearence.DocumentNo ) )
        {
          string _msg = "IPR record with the same SAD document number: {0} exist";
          throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _msg, clearence.DocumentNo ) );
        }
        _at = "newIPRData";
        comments = "Inconsistent or incomplete data to create IPR account";
        IPRAccountData _iprdata = new IPRAccountData( entities, clearence, ImportXMLCommon.Convert2MessageType( messageType ) );
        ErrorsList warnings = new ErrorsList();
        if ( !_iprdata.Validate( warnings ) )
          throw new InputDataValidationException( "Inconsistent or incomplete data to create IPR account", "Create IPR Account", warnings );
        comments = "Consent lookup filed";
        _at = "new IPRClass";
        IPRClass _ipr = new IPRClass( entities, _iprdata, clearence, declaration );
        _at = "new InsertOnSubmit";
        entities.IPR.InsertOnSubmit( _ipr );
        clearence.Status = true;
        _at = "new SubmitChanges #1";
        entities.SubmitChanges();
        _ipr.UpdateTitle();
        _at = "new SubmitChanges #2";
        entities.SubmitChanges();
      }
      catch ( InputDataValidationException )
      {
        throw;
      }
      catch ( GenericStateMachineEngine.ActionResult _ex )
      {
        _ex.Message.Insert( 0, String.Format( "Message={0}, Reference={1}; ", messageType, _referenceNumber ) );
        throw;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "CreateIPRAccount method error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, "IPR account creation error" );
      }
      comments = "IPR account created";
    }
    /// <summary>
    /// Creates the CW account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="comments">The comments.</param>
    /// <exception cref="InputDataValidationException">Create CW Account Failed;CreateCWAccount</exception>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    private static void CreateCWAccount( Entities entities, Clearence clearence, CustomsDocument.DocumentType messageType, out string comments )
    {
      string _at = "started";
      comments = "IPR account creation error";
      string _referenceNumber = String.Empty;
      try
      {
        SADDocumentType declaration = clearence.Clearence2SadGoodID.SADDocumentIndex;
        _referenceNumber = declaration.ReferenceNumber;
        _at = "newIPRData";
        comments = "Inconsistent or incomplete data to create IPR account";
        CWAccountData _accountData = new CWAccountData( entities, clearence, ImportXMLCommon.Convert2MessageType( messageType ) );
        comments = "Consent lookup filed";
        _at = "new IPRClass";
        ICWAccountFactory _cwFactory = CWClearanceHelpers.GetICWAccountFactory();
        List<Warnning> _lw = new List<Warnning>();
        _cwFactory.CreateCWAccount( _accountData, _lw, entities.Web );
        if ( _lw.Count == 0 )
        {
          comments = "CW account created";
          return;
        }
        ErrorsList _el = new ErrorsList();
        _el.Add( _lw );
        throw new InputDataValidationException( "Create CW Account Failed", "CreateCWAccount", _el );
        //clearence.Status = true;
        //_at = "new SubmitChanges #1";
        //entities.SubmitChanges();
        //_at = "new SubmitChanges #2";
        //entities.SubmitChanges();
      }
      catch ( InputDataValidationException )
      {
        throw;
      }
      catch ( GenericStateMachineEngine.ActionResult _ex )
      {
        _ex.Message.Insert( 0, String.Format( "Message={0}, Reference={1}; ", messageType, _referenceNumber ) );
        throw;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "CreateCWAccount method error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, "IPR account creation error" );
      }
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
          "clear through castoms fatal error", true );
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
