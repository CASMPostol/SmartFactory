﻿using System;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SADDocumentType
  {

    #region public
    internal void ReExportOfGoods(Entities _edc, xml.Customs.CustomsDocument.DocumentType _messageType)
    {
      this.SADDocument2Clearence = FimdClearence(_edc);
      foreach (SADGood _sg in SADGood)
      {
        if (_sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport)
          throw new IPRDataConsistencyException("Clearence.Create", String.Format("IE529 contains invalid customs procedure {0}", _sg.Title), null, "Wrong customs procedure.");
        //TODO [pr4-3719] Export: Association of the SAD documents - unique naming convention  http://itrserver/Bugs/BugDetail.aspx?bid=3719
        //TODO [pr4-3707] Export: Association of the SAD documents - SAD handling procedure modification http://itrserver/Bugs/BugDetail.aspx?bid=3707
        foreach ( SADRequiredDocuments _rdx in _sg.SADRequiredDocuments )
        {
          //if (_rdx.Code != 
        }
        throw new NotImplementedException();
      }
    }
    internal void ReleaseForFreeCirculation(Entities _edc, out string _comments)
    {
      //TODO NotImplementedException
      _comments = "NotImplementedException";
      throw new NotImplementedException() { Source = "ReleaseForFreeCirculation"};
    } 
    #endregion

    #region private
    private Clearence FimdClearence(Entities _edc)
    {
      //TODO NotImplementedException
      throw new NotImplementedException() { Source = "FimdClearence" };
    }
    #endregion
  }
}
