using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm;

namespace CAS.SmartFactory.IPR.Dashboards.Linq.DocumentsFactory
{
  internal static class DustWasteFormFactory
  {
    internal static DocumentContent DocumentContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo, DateTime endDate, DateTime startDate )
    {

      IQueryable<IGrouping<string, Disposal>> _groups = from _disx in disposals
                                                        let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                                                        orderby _ogl ascending
                                                        group _disx by _ogl;
      List<MaterialsOnOneAccount> _dustsGroupe = new List<MaterialsOnOneAccount>();
      double _total = 0;
      foreach ( IGrouping<string, Disposal> _gx in _groups )
      {
        double _subTotal = 0;
        IEnumerable<Disposal> _disposals = from _dspslx in _gx select _dspslx;
        _dustsGroupe.Add(
          new MaterialsOnOneAccount()
          {
            Total = _subTotal,
            MaterialRecords = Disposal.GetListOfMaterials( _disposals, _gx.Key, ref _subTotal ),
          }
        );
        _total += _subTotal;
      }
      return new DocumentContent()
      {
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentNo,
        AccountDescription = _dustsGroupe.ToArray(),
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
  }
}
