using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.TobaccoFreeCirculationForm;

namespace CAS.SmartFactory.Linq.IPR.DocumentsFactory
{
  internal static class TobaccoFreeCirculationFormFactory
  {
    internal static DocumentContent GetDocumentContent( IEnumerable<Disposal> disposals, string customProcedureCode, string documentNo )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( disposals, ref _subTotal );
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = ( from _dx in disposals select new { _endDate = _dx.Disposal2IPRIndex.CustomsDebtDate.Value } ).Max( x => x._endDate );
      DateTime startDate = ( from _dx in disposals select new { _endDate = _dx.Disposal2IPRIndex.CustomsDebtDate.Value } ).Max( x => x._endDate );
      return new DocumentContent()
      {
        AccountDescription = _materialRecords,
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _subTotal
      };
    }
  }
}
