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
                ReleaseForFreeCirculation(_edc, _sad);
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
          _ret = FimdClearence(_edc, _sad.ReferenceNumber);
          foreach (SADGood _sg in _sad.SADGood)
          {
            if (_sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport)
              throw new CustomsDataException("Clearence.Create", String.Format("IE529 contains invalid customs procedure {0}", _sg.Tytuł));
            ReExportOfGoods(_edc, _messageType, _sg);
          }
          break;
        case CustomsDocument.DocumentType.CLNE:
          _ret = FimdClearence(_edc, _sad.ReferenceNumber);
          _ret.DocumentNo = _sad.DocumentNumber;
          SADDocumentType _startingDocument = _ret.SADDocumentType.First<SADDocumentType>();
          switch (_ret.ProcedureCode.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              ReleaseForFreeCirculation(_edc, _startingDocument);
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
    private static void ReExportOfGoods(EntitiesDataContext _edc, CustomsDocument.DocumentType _documentType, SADGood _sg)
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
    private static void ReleaseForFreeCirculation(EntitiesDataContext _edc, SADDocumentType _sd)
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
  }
}
