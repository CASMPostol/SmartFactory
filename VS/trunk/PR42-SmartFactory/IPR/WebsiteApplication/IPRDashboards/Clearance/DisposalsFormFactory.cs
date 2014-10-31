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
    internal static DocumentContent GetBoxFormContent(IEnumerable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo)
    {
      DateTime endDate = disposals.Max(x => x.Disposal2IPRIndex.CustomsDebtDate.Value);
      DateTime startDate = disposals.Min(x => x.Disposal2IPRIndex.CustomsDebtDate.Value);
      MaterialsOnOneAccount _materials = CreateMaterialRecords(disposals);
      return new DocumentContent()
      {
        AccountDescription = new MaterialsOnOneAccount[] { _materials },
        CustomProcedureCode = Entities.ToString(customProcedureCode),
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentNo,
        EndDate = endDate.Date,
        StartDate = startDate.Date,
        TotalQuantity = _materials.TotalQuantity,
        TotalValue = _materials.TotalValue
      };
    }
    internal static DocumentContent GetTobaccoFreeCirculationFormContent(IEnumerable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo)
    {
      DateTime endDate = disposals.Max(x => x.Disposal2IPRIndex.CustomsDebtDate.Value);
      DateTime startDate = disposals.Min(x => x.Disposal2IPRIndex.CustomsDebtDate.Value);
      return CreateDocumentContent(disposals, customProcedureCode, documentNo, endDate, startDate);
    }
    internal static DocumentContent GetDustWasteFormContent(IEnumerable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo)
    {
      DateTime endDate = disposals.Max(x => x.Created.Value);
      DateTime startDate = disposals.Min(x => x.Created.Value);
      return CreateDocumentContent(disposals, customProcedureCode, documentNo, endDate, startDate);
    }
    #endregion

    #region private
    private static DocumentContent CreateDocumentContent(IEnumerable<Disposal> disposals, ClearenceProcedure customProcedureCode, string documentNo, DateTime endDate, DateTime startDate)
    {
      IEnumerable<IGrouping<string, Disposal>> _groups = from _disx in disposals
                                                         let _ogl = _disx.Disposal2IPRIndex == null ? String.Empty : _disx.Disposal2IPRIndex.DocumentNo
                                                         orderby _ogl ascending
                                                         group _disx by _ogl;
      List<MaterialsOnOneAccount> _group = new List<MaterialsOnOneAccount>();
      double _totalQuantity = 0;
      double _totalValue = 0;
      foreach (IGrouping<string, Disposal> _gx in _groups)
      {
        IEnumerable<Disposal> _dspslsInGroup = from _dspslx in _gx select _dspslx;
        MaterialsOnOneAccount _mona = CreateMaterialRecords(_dspslsInGroup);
        _group.Add(_mona);
        _totalValue += _mona.TotalValue;
        _totalQuantity += _mona.TotalQuantity;
      }
      return new DocumentContent()
      {
        AccountDescription = _group.ToArray(),
        CustomProcedureCode = Entities.ToString(customProcedureCode),
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentNo,
        EndDate = endDate.Date,
        StartDate = startDate.Date,
        TotalQuantity = _totalQuantity,
        TotalValue = _totalValue
      };
    }
    private static MaterialsOnOneAccount CreateMaterialRecords(IEnumerable<Disposal> dspslsInGroup)
    {
      double _subTotalQuantity = 0;
      double _subTotalValue = 0;
      MaterialRecord[] _materialRecords = GetListOfMaterials(dspslsInGroup, ref _subTotalQuantity, ref _subTotalValue);
      return new MaterialsOnOneAccount()
        {
          TotalQuantity = _subTotalQuantity,
          TotalValue = _subTotalValue,
          MaterialRecords = _materialRecords,
        };
    }
    private static MaterialRecord[] GetListOfMaterials(IEnumerable<Disposal> disposals, ref double subTotalQuantity, ref double subTotalValue)
    {
      List<MaterialRecord> _dustRecord = new List<MaterialRecord>();
      foreach (Disposal _dx in disposals)
      {
        subTotalQuantity += _dx.SettledQuantity.Value;
        subTotalValue += _dx.TobaccoValue.GetValueOrDefault(0);
        MaterialRecord _newRecord = new MaterialRecord()
        {
          Qantity = _dx.SettledQuantity.GetValueOrDefault(0),
          Date = _dx.Disposal2IPRIndex.CustomsDebtDate.GetValueOrNull(),
          CustomDocumentNo = _dx.Disposal2IPRIndex.DocumentNo,
          FinishedGoodBatch = _dx.Disposal2BatchIndex == null ? String.Empty : _dx.Disposal2BatchIndex.Batch0,
          MaterialBatch = _dx.Disposal2IPRIndex.Batch,
          MaterialSKU = _dx.Disposal2IPRIndex.SKU,
          UnitPrice = _dx.Disposal2IPRIndex.IPRUnitPrice.GetValueOrDefault(0),
          TobaccoValue = _dx.TobaccoValue.GetValueOrDefault(0),
          Currency = _dx.Disposal2IPRIndex.Currency
        };
        _dustRecord.Add(_newRecord);
      }
      return _dustRecord.ToArray();
    }
    #endregion

  }
}
