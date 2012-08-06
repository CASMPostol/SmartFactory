using System;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Clearence
  {
    internal static Clearence Associate( EntitiesDataContext _edc, CustomsDocument.DocumentType _messageType, SADDocumentType _sad, out string _comments, SADDocumentLib customsDocumentLibrary )
    {
      Clearence _ret = default( Clearence );
      string _at = "started";
      _comments = "Clearance association error";
      try
      {
        switch ( _messageType )
        {
          case CustomsDocument.DocumentType.SAD:
          case CustomsDocument.DocumentType.PZC:
            _at = "_customsProcedureCodes";
            CustomsProcedureCodes _customsProcedureCodes = _sad.SADGood.First().Procedure.RequestedProcedure();
            switch ( _customsProcedureCodes )
            {
              case CustomsProcedureCodes.FreeCirculation:
                _at = "FimdClearence";
                _ret = FimdClearence( _edc, _sad.ReferenceNumber );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                {
                  _sad.ReleaseForFreeCirculation( _edc, out _comments );
                }
                else
                  _comments = "Document added";
                break;
              case CustomsProcedureCodes.InwardProcessing:
                _at = "_procedureCode";
                string _procedureCode = String.Format( "{0:D2}XX", (int)_customsProcedureCodes );
                _at = "NewClearence";
                _ret = new Clearence()
                {
                  DocumentNo = _sad.DocumentNumber,
                  ReferenceNumber = _sad.ReferenceNumber,
                  SADConsignmentLibraryLookup = null,
                  ProcedureCode = _procedureCode,
                  Status = false,
                  Procedure = Linq.IPR.Procedure._5171,
                  Tytuł = String.Format( "Procedure {0} Ref: {1}", _procedureCode, _sad.ReferenceNumber )
                };
                _at = "InsertOnSubmit";
                _edc.Clearence.InsertOnSubmit( _ret );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                {
                  IPR.CreateIPRAccount( _edc, _sad, _ret, CustomsDocument.DocumentType.PZC, _sad.CustomsDebtDate.Value, out _comments, customsDocumentLibrary );
                }
                else
                  _comments = "Document added";
                break;
              case CustomsProcedureCodes.NoProcedure:
              case CustomsProcedureCodes.ReExport:
              case CustomsProcedureCodes.CustomsWarehousingProcedure:
              default:
                throw new IPRDataConsistencyException( "Clearence.Associate", string.Format( "Unexpected procedure code for the {0} message", _messageType ), null, _wrongProcedure );
            } //switch (_customsProcedureCodes)
            break;
          case CustomsDocument.DocumentType.IE529:
            _comments = "Reexport of goods failed";
            _sad.ReExportOfGoods( _edc, _messageType );
            _comments = "Reexport of goods";
            break;
          case CustomsDocument.DocumentType.CLNE:
            _at = "FimdClearence";
            _ret = FimdClearence( _edc, _sad.ReferenceNumber );
            _ret.DocumentNo = _sad.DocumentNumber;
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
      catch ( Exception _ex )
      {
        string _src = String.Format( "Clearence analyses error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
      return _ret;
    }
    private static Clearence FimdClearence( EntitiesDataContext _edc, string _referenceNumber )
    {
      return ( from _cx in _edc.Clearence where _referenceNumber.Contains( _cx.ReferenceNumber ) select _cx ).First<Clearence>();
    }
    private const string _wrongProcedure = "Wrong customs procedure";
  }
}
