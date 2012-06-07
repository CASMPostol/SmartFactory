using System;
using System.Linq;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Clearence
  {
    internal static Clearence Associate(EntitiesDataContext _edc, CustomsDocument.DocumentType _messageType, SADDocumentType _sad)
    {
      Clearence _ret = default(Clearence);
      switch (_messageType)
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          CustomsProcedureCodes _cpc = _sad.SADGood.First().Procedure.RequestedProcedure();
          switch (_cpc)
          {
            case CustomsProcedureCodes.FreeCirculation:
              _ret = FimdClearence(_edc, _sad.ReferenceNumber);
              if (_messageType == CustomsDocument.DocumentType.PZC)
                _sad.ReleaseForFreeCirculation(_edc);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              string _procedureCode = String.Format("{0:D2}XX", (int)_cpc);
              _ret = new Clearence()
              {
                DocumentNo = _sad.DocumentNumber,
                ReferenceNumber = _sad.ReferenceNumber,
                SADConsignmentLibraryLookup = null,
                ProcedureCode = _procedureCode,
                Status = false,
                Tytuł = String.Format("Procedure {0} Ref: {1}", _procedureCode, _sad.ReferenceNumber)
              };
              _edc.Clearence.InsertOnSubmit(_ret);
              if (_messageType == CustomsDocument.DocumentType.PZC)
                IPR.CreateIPRAccount(_edc, _sad, _ret, CustomsDocument.DocumentType.PZC);
              break;
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.ReExport:
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
            default:
              throw new CustomsDataException("Clearence.Associate", string.Format("Unexpected procedure code for the {0} message", _messageType));
          }
          break;
        case CustomsDocument.DocumentType.IE529:
          _sad.ReExportOfGoods(_edc, _messageType);
          break;
        case CustomsDocument.DocumentType.CLNE:
          _ret = FimdClearence(_edc, _sad.ReferenceNumber);
          _ret.DocumentNo = _sad.DocumentNumber;
          SADDocumentType _startingDocument = _ret.SADDocumentType.First<SADDocumentType>();
          switch (_ret.ProcedureCode.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              _startingDocument.ReleaseForFreeCirculation(_edc);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              IPR.CreateIPRAccount(_edc, _startingDocument, _ret, CustomsDocument.DocumentType.SAD);
              break;
            case CustomsProcedureCodes.ReExport:
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
            default:
              throw new CustomsDataException("Clearence.Associate", "Unexpected procedure code for CLNE message");
          }
          break;
        default:
          break;
      }//switch (_documentType
      return _ret;
    }
    private static Clearence FimdClearence(EntitiesDataContext _edc, string _referenceNumber)
    {
      return (from _cx in _edc.Clearence where _referenceNumber.Contains(_cx.ReferenceNumber) select _cx).First<Clearence>();
    }
  }
}
