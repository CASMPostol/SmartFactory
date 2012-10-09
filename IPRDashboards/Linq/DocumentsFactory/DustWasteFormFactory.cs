using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm;

namespace CAS.SmartFactory.Linq.IPR.DocumentsFactory
{
  internal static class DustWasteFormFactory
  {
    internal static DocumentContent GetDocumentContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo )
    {

      IQueryable<IGrouping<string, Disposal>> _groups = from _disx in disposals
                                                        let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                                                        orderby _ogl ascending
                                                        group _disx by _ogl;
      //TODO Disposal contenty type does not provides creation and modification data
      DateTime endDate = (from _dx in disposals select new {_endDate = DateTime.Today.Date}).Max(x=>x._endDate);
      DateTime startDate = ( from _dx in disposals select new { _endDate = DateTime.Today.Date } ).Max( x => x._endDate );
      List<MaterialsOnOneAccount> _dustsGroupe = new List<MaterialsOnOneAccount>();
      double _total = 0;
      foreach ( IGrouping<string, Disposal> _gx in _groups )
      {
        double _subTotal = 0;
        IEnumerable<Disposal> _dspslsInGroup = from _dspslx in _gx select _dspslx;
        MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( _dspslsInGroup, ref _subTotal );
        _dustsGroupe.Add(
          new MaterialsOnOneAccount()
          {
            Total = _subTotal,
            MaterialRecords = _materialRecords,
          }
        );
        _total += _subTotal;
      }
      return new DocumentContent()
      {
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date, //TODO ?????
        DocumentNo = documentNo,
        AccountDescription = _dustsGroupe.ToArray(),
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
  }
}
