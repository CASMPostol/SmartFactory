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
    internal static DocumentContent GetBoxFormContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo )
    {
      //TODO not implemented
      throw new NotImplementedException();
    }
    internal static CAS.SmartFactory.xml.DocumentsFactory.TobaccoFreeCirculationForm.DocumentContent GetTobaccoFreeCirculationFormContent( IEnumerable<Disposal> disposals, string customProcedureCode, string documentNo )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( disposals, ref _subTotal );
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = ( from _dx in disposals select new { _endDate = _dx.Disposal2IPRIndex.CustomsDebtDate.Value } ).Max( x => x._endDate );
      DateTime startDate = ( from _dx in disposals select new { _endDate = _dx.Disposal2IPRIndex.CustomsDebtDate.Value } ).Max( x => x._endDate );
      return new CAS.SmartFactory.xml.DocumentsFactory.TobaccoFreeCirculationForm.DocumentContent()
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
    internal static DocumentContent GetDustWasteFormContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo )
    {

      IQueryable<IGrouping<string, Disposal>> _groups = from _disx in disposals
                                                        let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                                                        orderby _ogl ascending
                                                        group _disx by _ogl;
      DateTime endDate = (from _dx in disposals select new {_endDate = _dx.CreatedDate.Value}).Max(x=>x._endDate);
      DateTime startDate = ( from _dx in disposals select new { _endDate = _dx.CreatedDate.Value } ).Max( x => x._endDate );
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
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        AccountDescription = _dustsGroupe.ToArray(),
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
  }
}
