using System;
using System.Collections.Generic;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.TobaccoFreeCirculationForm;

namespace CAS.SmartFactory.IPR.Dashboards.Linq.DocumentsFactory
{
  internal static class TobaccoFreeCirculationFormFactory
  {
    internal static DocumentContent CreateTobaccoFreeCirculationForm( IEnumerable<Disposal> disposals, string customProcedureCode, string documentNo, DateTime endDate, DateTime startDate )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( disposals, ref _subTotal );
      return new DocumentContent()
      {
        AccountDescription = _materialRecords,
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date, //TODO ???
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _subTotal
      };
    }
  }
}
