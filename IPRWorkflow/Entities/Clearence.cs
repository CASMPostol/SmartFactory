using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Clearence
  {
    internal static Clearence Associate(EntitiesDataContext _edc, CustomsDocument.DocumentType _documentType, SADDocumentType _sad)
    {
      Clearence _ret = default(Clearence);
      switch (_documentType)
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          CustomsProcedureCodes _cpc = _sad.GetCustomsProcedureCodes();
          switch (_cpc)
          {
            case CustomsProcedureCodes.FreeCirculation:
              _ret = FimdClearence(_edc, _sad.ReferenceNumber);
              if (_documentType == CustomsDocument.DocumentType.PZC)
                ReleaseForFreeCirculation(_edc, _sad);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              _ret = new Clearence()
              {
                DocumentNo = String.Empty,
                ReferenceNumber = _sad.ReferenceNumber,
                SADConsignmentLibraryLookup = null,
                ProcedureCode = String.Format("{0,2}XX", (int)_cpc),
                Status = false,
                Tytuł = String.Format("SAD Ref: {0}", _sad.ReferenceNumber)
              };
              _edc.Clearence.InsertOnSubmit(_ret);
              if (_documentType == CustomsDocument.DocumentType.PZC)
                IPR.CreateIPRAccount(_edc, _sad, _ret);
              break;
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.ReExport:
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
            default:
              throw new CustomsDataException("Clearence.Associate", string.Format("Unexpected procedure code for the {0} message", _documentType));
          }
          break;
        case CustomsDocument.DocumentType.IE529:
          _ret = FimdClearence(_edc, _sad.ReferenceNumber);
          foreach (SADGood _sg in _sad.SADGood)
          {
            if (_sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport)
              throw new CustomsDataException("Clearence.Create", String.Format("IE529 contains invalid customs procedure {0}", _sg.Tytuł));
            ReExportOfGoods(_edc, _documentType, _sg);
          }
          break;
        case CustomsDocument.DocumentType.CLNE:
          _ret = FimdClearence(_edc, _sad.ReferenceNumber);
          _ret.DocumentNo = _sad.DocumentNumber;
          SADDocumentType _startingDocument = _ret.GetSADDocument();
          switch (_ret.ProcedureCode.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              ReleaseForFreeCirculation(_edc, _startingDocument);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              IPR.CreateIPRAccount(_edc, _startingDocument, _ret);
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

    /// <summary>
    /// Gets the SAD document.
    /// </summary>
    /// <returns></returns>
    private SADDocumentType GetSADDocument()
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
    private static Clearence FimdClearence(EntitiesDataContext _edc, string p)
    {
      throw new NotImplementedException();
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
