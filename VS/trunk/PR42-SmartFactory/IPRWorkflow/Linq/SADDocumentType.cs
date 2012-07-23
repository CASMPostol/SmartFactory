using System;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SADDocumentType
  {

    #region public
    internal void ReExportOfGoods(EntitiesDataContext _edc, xml.Customs.CustomsDocument.DocumentType _messageType)
    {
      ClearenceID = FimdClearence(_edc);
      foreach (SADGood _sg in SADGood)
      {
        if (_sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport)
          throw new IPRDataConsistencyException("Clearence.Create", String.Format("IE529 contains invalid customs procedure {0}", _sg.Tytuł), null, "Wrong customs procedure.");
        throw new NotImplementedException();
      }
    }
    internal void ReleaseForFreeCirculation(EntitiesDataContext _edc, out string _comments)
    {
      //TODO NotImplementedException
      _comments = "NotImplementedException";
      throw new NotImplementedException() { Source = "ReleaseForFreeCirculation"};
    } 
    #endregion

    #region private
    private Clearence FimdClearence(EntitiesDataContext _edc)
    {
      //TODO NotImplementedException
      throw new NotImplementedException() { Source = "FimdClearence" };
    }
    #endregion
  }
}
