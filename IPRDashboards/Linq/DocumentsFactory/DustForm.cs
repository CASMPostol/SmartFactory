using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml.DocumentsFactory.DustForm;

namespace CAS.SmartFactory.IPR.Dashboards.Linq.DocumentsFactory
{
  internal static class DustFormFactory
  {
    internal static xml.DocumentsFactory.DustForm.DocumentContent DocumentContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo, DateTime endDate, DateTime startDate )
    {

      var _groups = from _disx in disposals
                    let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                    orderby _ogl ascending
                    group _disx by _ogl;
      List<xml.DocumentsFactory.DustForm.DustsOnOneAccount> _dustsGroupe = new List<DustsOnOneAccount>();
      double _total = 0;
      foreach ( IGrouping<string, Disposal> _gx in _groups )
      {
        double _subTotal = 0;
        List<xml.DocumentsFactory.DustForm.DustRecord> _dustRecord = new List<DustRecord>();
        foreach ( Disposal _dx in _gx )
        {
          _subTotal += _dx.SettledQuantity.Value;
          DustRecord _newRecord = new DustRecord()
           {
             Amount = _dx.SettledQuantity.GetValueOrDefault( 0 ),
             //TODO not sure about if this parameter is set corectly 
             Date = _dx.Disposal2IPRIndex.CustomsDebtDate.GetValueOrNull(),
             CustomDocumentNo = _gx.Key,
             FinishedGoodBatch = _dx.Disposal2BatchIndex.Batch0,
             MaterialBatch = _dx.Disposal2IPRIndex.Batch,
             MaterialSKU = _dx.Disposal2IPRIndex.SKU
           };
          _dustRecord.Add( _newRecord );
        }
        _dustsGroupe.Add(
          new DustsOnOneAccount()
          {
            Total = _subTotal,
            DustRecord = _dustRecord.ToArray(),
          }
        );
        _total += _subTotal;
      }
      return new DocumentContent()
      {
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentNo,
        Dusts = _dustsGroupe.ToArray(),
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
  }
}
