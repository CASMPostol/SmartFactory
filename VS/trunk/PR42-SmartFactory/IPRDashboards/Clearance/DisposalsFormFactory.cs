using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;


namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{
  internal static class DisposalsFormFactory
  {

    #region internal
    internal static DocumentContent GetBoxFormContent( IQueryable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo )
    {
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = disposals.Max( x => x.Created.Value );
      DateTime startDate = disposals.Max( x => x.Created.Value );
      MaterialsOnOneAccount _materials = CreateMaterialRecords( disposals );
      return new DocumentContent()
      {
        AccountDescription = new MaterialsOnOneAccount[] { _materials },
        CustomProcedureCode = Entities.ToString( customProcedureCode ),
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _materials.Total
      };
    }
    internal static DocumentContent GetTobaccoFreeCirculationFormContent( IQueryable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo )
    {
      //TODO not sure about how to calculate end and start date 
      DateTime endDate = disposals.Max( x => x.Disposal2IPRIndex.CustomsDebtDate.Value );
      DateTime startDate = disposals.Max( x => x.Disposal2IPRIndex.CustomsDebtDate.Value );
      return CreateDocumentContent( disposals, customProcedureCode, documentNo, endDate, startDate );
    }
    internal static DocumentContent GetDustWasteFormContent( IQueryable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo )
    {
      DateTime endDate = disposals.Max( x => x.Created.Value );
      DateTime startDate = disposals.Max( x => x.Created.Value );
      return CreateDocumentContent( disposals, customProcedureCode, documentNo, endDate, startDate );
    }
    #endregion

    #region private
    private static DocumentContent CreateDocumentContent( IQueryable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo, DateTime endDate, DateTime startDate )
    {
      IQueryable<IGrouping<string, Disposal>> _groups = from _disx in disposals
                                                        let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                                                        orderby _ogl ascending
                                                        group _disx by _ogl;
      List<MaterialsOnOneAccount> _group = new List<MaterialsOnOneAccount>();
      double _total = 0;
      foreach ( IGrouping<string, Disposal> _gx in _groups )
      {
        IEnumerable<Disposal> _dspslsInGroup = from _dspslx in _gx select _dspslx;
        MaterialsOnOneAccount _mona = CreateMaterialRecords( _dspslsInGroup );
        _group.Add( _mona );
        _total += _mona.Total;
      }
      return new DocumentContent()
      {
        AccountDescription = _group.ToArray(),
        CustomProcedureCode = Entities.ToString( customProcedureCode ),
        DocumentDate = DateTime.Today.Date, //TODO not sure how to assigne document date.
        DocumentNo = documentNo,
        EndDate = endDate,
        StartDate = startDate,
        Total = _total
      };
    }
    private static MaterialsOnOneAccount CreateMaterialRecords( IEnumerable<Disposal> _dspslsInGroup )
    {
      double _subTotal = 0;
      MaterialRecord[] _materialRecords = GetListOfMaterials( _dspslsInGroup, ref _subTotal );
      return new MaterialsOnOneAccount()
        {
          Total = _subTotal,
          MaterialRecords = _materialRecords,
        };
    }
    private static MaterialRecord[] GetListOfMaterials( IEnumerable<Disposal> _disposals, ref double _subTotal )
    {
      List<MaterialRecord> _dustRecord = new List<MaterialRecord>();
      foreach ( Disposal _dx in _disposals )
      {
        _subTotal += _dx.SettledQuantity.Value;
        MaterialRecord _newRecord = new MaterialRecord()
        {
          Qantity = _dx.SettledQuantity.GetValueOrDefault( 0 ),
          Date = _dx.Disposal2IPRIndex.CustomsDebtDate.GetValueOrNull(),
          CustomDocumentNo = _dx.Disposal2IPRIndex.DocumentNo,
          FinishedGoodBatch = _dx.Disposal2BatchIndex == null ? String.Empty : _dx.Disposal2BatchIndex.Batch0,
          MaterialBatch = _dx.Disposal2IPRIndex.Batch,
          MaterialSKU = _dx.Disposal2IPRIndex.SKU,
          UnitPrice = _dx.Disposal2IPRIndex.IPRUnitPrice.Value,
          TobaccoValue = _dx.TobaccoValue.Value,
          Currency = _dx.Disposal2IPRIndex.Currency
        };
        _dustRecord.Add( _newRecord );
      }
      return _dustRecord.ToArray();
    }
    #endregion

  }
}
