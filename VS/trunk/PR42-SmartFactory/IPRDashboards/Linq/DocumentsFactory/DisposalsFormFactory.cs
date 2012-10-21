﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;

namespace CAS.SmartFactory.Linq.IPR.DocumentsFactory
{
  internal static class DisposalsFormFactory
  {
    internal static DocumentContent GetBoxFormContent( IQueryable<Disposal> disposals, string customProcedureCode, string documentNo )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( disposals, ref _subTotal );
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = disposals.Max( x => x.CreatedDate.Value );
      DateTime startDate = disposals.Max( x => x.CreatedDate.Value );
      MaterialsOnOneAccount _materials = new MaterialsOnOneAccount()
      {
        Total = _subTotal,
        MaterialRecords = _materialRecords,
      };
      return new DocumentContent()
      {
        AccountDescription = new MaterialsOnOneAccount[] { _materials },
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _subTotal
      };
    }
    internal static DocumentContent GetTobaccoFreeCirculationFormContent( IEnumerable<Disposal> disposals, string customProcedureCode, string documentNo )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = Disposal.GetListOfMaterials( disposals, ref _subTotal );
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = disposals.Max( x => x.Disposal2IPRIndex.CustomsDebtDate.Value );
      DateTime startDate = disposals.Max( x => x.Disposal2IPRIndex.CustomsDebtDate.Value );
      MaterialsOnOneAccount _materials = new MaterialsOnOneAccount()
      {
        Total = _subTotal,
        MaterialRecords = _materialRecords,
      };
      return new DocumentContent()
      {
        AccountDescription = new MaterialsOnOneAccount[] { _materials },
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
      DateTime endDate = disposals.Max( x => x.CreatedDate.Value );
      DateTime startDate = disposals.Max( x => x.CreatedDate.Value );
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
        AccountDescription = _dustsGroupe.ToArray(),
        CustomProcedureCode = customProcedureCode,
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
  }
}
