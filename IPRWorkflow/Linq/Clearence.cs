﻿using System;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Customs;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Clearence
  {
    internal static Clearence Associate( Entities _edc, CustomsDocument.DocumentType _messageType, SADDocumentType _sad, out string _comments, SADDocumentLib customsDocumentLibrary )
    {
      Clearence _ret = default( Clearence );
      string _at = "started";
      _comments = "Clearance association error";
      try
      {
        //TODO common naming mechanism must implemented
        switch ( _messageType )
        {
          case CustomsDocument.DocumentType.SAD:
          case CustomsDocument.DocumentType.PZC:
            _at = "_customsProcedureCodes";
            CustomsProcedureCodes _customsProcedureCodes = _sad.SADGood.First().Procedure.RequestedProcedure();
            string _procedureCodeFormat = "{0:D2}XX";
            switch ( _customsProcedureCodes )
            {
              case CustomsProcedureCodes.FreeCirculation:
                _at = "FimdClearence";
                _ret = FimdClearence( _edc, _sad.ReferenceNumber );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  _sad.ReleaseForFreeCirculation( _edc, out _comments );
                else
                  _comments = "Document added";
                _ret.CreateTitle( _messageType.ToString() );
                break;
              case CustomsProcedureCodes.InwardProcessing:
                _at = "NewClearence";
                _ret = new Clearence()
                {
                  DocumentNo = _sad.DocumentNumber,
                  ReferenceNumber = _sad.ReferenceNumber,
                  SADConsignmentLibraryIndex = null,
                  ProcedureCode = String.Format( _procedureCodeFormat, (int)_customsProcedureCodes ),
                  Status = false,
                  ClearenceProcedure = Linq.IPR.ClearenceProcedure._5171,
                };
                _at = "InsertOnSubmit";
                _edc.Clearence.InsertOnSubmit( _ret );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  IPR.CreateIPRAccount( _edc, _sad, _ret, CustomsDocument.DocumentType.PZC, _sad.CustomsDebtDate.Value, out _comments, customsDocumentLibrary );
                else
                  _comments = "Document added";
                _ret.CreateTitle( _messageType.ToString() );
                break;
              case CustomsProcedureCodes.CustomsWarehousingProcedure:
                _at = "NewClearence";
                _ret = new Clearence()
                {
                  DocumentNo = _sad.DocumentNumber,
                  ReferenceNumber = _sad.ReferenceNumber,
                  SADConsignmentLibraryIndex = null,
                  ProcedureCode = String.Format( _procedureCodeFormat, (int)_customsProcedureCodes ),
                  Status = false,
                  //TODO We need procedure 7100
                  ClearenceProcedure = Linq.IPR.ClearenceProcedure._7171,
                };
                _ret.CreateTitle( _messageType.ToString() );
                _at = "InsertOnSubmit";
                _edc.Clearence.InsertOnSubmit( _ret );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  ;// TODO CreateStockRecord  
                else
                  _comments = "Document added";
                break;
              case CustomsProcedureCodes.NoProcedure:
              case CustomsProcedureCodes.ReExport:
              default:
                throw new IPRDataConsistencyException( "Clearence.Associate", string.Format( "Unexpected procedure code for the {0} message", _messageType ), null, _wrongProcedure );
            } //switch (_customsProcedureCodes)
            break;
          case CustomsDocument.DocumentType.IE529:
            _at = "ReExportOfGoods";
            _comments = "Reexport of goods failed";
            _sad.ReExportOfGoods( _edc, _messageType );
            _comments = "Reexport of goods";
            break;
          case CustomsDocument.DocumentType.CLNE:
            _at = "FimdClearence";
            _ret = FimdClearence( _edc, _sad.ReferenceNumber );
            _ret.DocumentNo = _sad.DocumentNumber;
            _ret.CreateTitle( _messageType.ToString() );
            _at = "StartingDocument";
            SADDocumentType _startingDocument = _ret.SADDocumentType.First<SADDocumentType>();
            _at = "switch RequestedProcedure";
            switch ( _ret.ProcedureCode.RequestedProcedure() )
            {
              case CustomsProcedureCodes.FreeCirculation:
                _startingDocument.ReleaseForFreeCirculation( _edc, out _comments );
                break;
              case CustomsProcedureCodes.InwardProcessing:
                IPR.CreateIPRAccount( _edc, _startingDocument, _ret, CustomsDocument.DocumentType.SAD, _sad.CustomsDebtDate.Value, out _comments, customsDocumentLibrary );
                break;
              case CustomsProcedureCodes.ReExport:
              case CustomsProcedureCodes.NoProcedure:
              case CustomsProcedureCodes.CustomsWarehousingProcedure:
              default:
                throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected procedure code for CLNE message", null, _wrongProcedure );
            }
            break;
          default:
            throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected message type.", null, "Unexpected message type." );
        }//switch (_documentType
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
      return _ret;
    }
    private static Clearence FimdClearence( Entities _edc, string _referenceNumber )
    {
      return ( from _cx in _edc.Clearence where _referenceNumber.Contains( _cx.ReferenceNumber ) select _cx ).First<Clearence>();
    }
    private const string _wrongProcedure = "Wrong customs procedure";
  }
}
