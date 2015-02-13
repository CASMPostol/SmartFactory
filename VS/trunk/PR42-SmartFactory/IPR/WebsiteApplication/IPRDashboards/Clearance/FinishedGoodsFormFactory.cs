//<summary>
//  Title   : class FinishedGoodsFormFactory
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint;
using CAS.SharePoint.Logging;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{

  /// <summary>
  /// Batch Extension
  /// </summary>
  internal static class FinishedGoodsFormFactory
  {
    #region public
    internal static CigaretteExportFormCollection GetFormContent(Entities entities, InvoiceLib invoice, Clearence clearance, string documentName, int sadConsignmentNumber)
    {
      int _position = 1;
      List<CigaretteExportForm> _consignment = new List<CigaretteExportForm>();
      invoice.ClearenceIndex = clearance;
      invoice.InvoiceLibraryReadOnly = true;
      foreach (InvoiceContent item in invoice.InvoiceContent(entities))
        ExportInvoiceEntry(entities, item, _consignment, documentName, ref _position, sadConsignmentNumber);
      return GetCigaretteExportFormCollection(_consignment, documentName, invoice.BillDoc);
    }
    #endregion

    #region private forms factory
    internal static CigaretteExportFormCollection GetCigaretteExportFormCollection(List<CigaretteExportForm> forms, string documentName, string invoiceNo)
    {
      CigaretteExportFormCollection _ret = new CigaretteExportFormCollection()
      {
        CigaretteExportForms = forms.OrderBy<CigaretteExportForm, string>(x => x.DocumentNo).ToArray<CigaretteExportForm>(),
        DocumentNo = documentName,
        DocumentDate = DateTime.Today.Date,
        InvoiceNo = invoiceNo,
        NumberOfDocuments = forms.Count
      };
      return _ret;
    }
    internal static CigaretteExportForm GetCigaretteExportForm(Batch batch, InvoiceContent invoice, List<Ingredient> ingredients, string documentName, ref int subdocumentNo, ClearenceProcedure procedure)
    {
      double _portion = invoice.Quantity.Value / batch.FGQuantity.Value;
      CigaretteExportForm _ret = new CigaretteExportForm();
      if (batch == null)
        throw new ArgumentNullException("Batch cannot be null");
      if (batch.SKUIndex == null)
        throw new ArgumentNullException("SKU in batch cannot be null");
      if (invoice == null)
        throw new ArgumentNullException("Invoice cannot be null");
      _ret.DocumentNo = String.Format(GlobalDefinitions.CigaretteExportFormNamePatern, documentName, subdocumentNo++);
      _ret.DustKg = (batch.Dust.GetValueOrDefault(-1) * _portion).RountMass();
      CountExportFormTotals(ingredients, _ret);
      _ret.Portion = _portion;
      _ret.CustomsProcedure = Entities.ToString(procedure);
      _ret.FinishedGoodBatch = batch.Batch0;
      //TODO Must be calculated depending on commodity or unit.
      _ret.FinishedGoodQantity = invoice.Quantity.GetValueOrDefault(0);
      _ret.FinishedGoodUnit = invoice.Units.GetLocalizedString();
      _ret.FinishedGoodSKU = batch.SKUIndex.SKU;
      _ret.FinishedGoodSKUDescription = batch.SKUIndex.Title();
      _ret.MaterialTotal = (batch.Tobacco.GetValueOrDefault(-1) * _portion).RountMass();
      _ret.ProductFormat = batch.SKUIndex.FormatIndex.Title();
      _ret.CTFUsageMin = batch.CFTProductivityRateMin.GetValueOrDefault(-1) * 100;
      _ret.CTFUsageMax = batch.CFTProductivityRateMax.GetValueOrDefault(-1) * 100;
      _ret.CTFUsagePerUnitMin = batch.CFTProductivityRateMin.GetValueOrDefault(-1);
      _ret.CTFUsagePerUnitMax = batch.CFTProductivityRateMax.GetValueOrDefault(-1);
      _ret.CTFUsagePer1MFinishedGoodsMin = batch.CTFUsageMin.GetValueOrDefault(-1);
      _ret.CTFUsagePer1MFinishedGoodsMax = batch.CTFUsageMax.GetValueOrDefault(-1);
      _ret.WasteCoefficient = batch.BatchWasteCooeficiency.GetValueOrDefault(-1) + batch.BatchDustCooeficiency.GetValueOrDefault(-1);
      switch (batch.ProductType.Value)
      {
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cutfiller:
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cutfiller;
          break;
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette:
          SKUCigarette _skuCigarette = batch.SKUIndex as SKUCigarette;
          _ret.BrandDescription = _skuCigarette.Brand;
          _ret.FamilyDescription = _skuCigarette.Family;
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cigarette;
          break;
        default:
          throw new ApplicationError("InvoiceLib.CigaretteExportForm", "Product", "Wrong ProductType", null);
      }
      _ret.SHMentholKg = (batch.SHMenthol.GetValueOrDefault(-1) * _portion).RountMass();
      _ret.WasteKg = (batch.Waste.GetValueOrDefault(-1) * _portion).RountMass();
      _ret.IPRRestMaterialQantityTotal = _ret.DustKg + _ret.SHMentholKg + _ret.WasteKg;
      _ret.TobaccoTotal = (_ret.IPTMaterialQuantityTotal + _ret.RegularMaterialQuantityTotal + _ret.IPRRestMaterialQantityTotal).RountMass();
      return _ret;
    }
    internal static IPRIngredient GetIPRIngredient(Disposal disposal)
    {
      IPRIngredient _ret = new IPRIngredient(disposal.Disposal2IPRIndex.Batch, disposal.Disposal2IPRIndex.SKU, disposal.SettledQuantity.Value);
      CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR _ipr = disposal.Disposal2IPRIndex;
      _ret.Currency = _ipr.Currency;
      _ret.Date = _ipr.CustomsDebtDate.Value;
      _ret.DocumentNoumber = _ipr.DocumentNo;
      _ret.Duty = disposal.DutyPerSettledAmount.Value;
      switch (disposal.ClearingType.Value)
      {
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.TotalWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        default:
          throw new ApplicationError("InvoiceLib.IPRIngredient", "ItemClearingType", "Wrong Clearing Type", null);
      }
      _ret.TobaccoUnitPrice = _ipr.IPRUnitPrice.Value;
      _ret.TobaccoValue = disposal.TobaccoValue.Value;
      _ret.VAT = disposal.VATPerSettledAmount.Value;
      return _ret;
    }
    #endregion

    #region private
    private static void ExportInvoiceEntry
      (Entities entities, InvoiceContent invoice, List<CigaretteExportForm> formsList, string documentName, ref int subdocumentNo, int sadConsignmentNumber)
    {
      string _at = "beginning";
      Batch batch = invoice.InvoiceContent2BatchIndex;
      try
      {
        bool _closingBatch = batch.FGQuantityAvailable == invoice.Quantity.Value;
        _at = "FGQuantityAvailable";
        batch.FGQuantityAvailable = Convert.ToDouble(Convert.ToDecimal(batch.FGQuantityAvailable.Value) - Convert.ToDecimal(invoice.Quantity.Value));
        List<Ingredient> _ingredients = new List<Ingredient>();
        _at = "foreach";
        foreach (Material _materialIdx in batch.Material(entities, false))
          ExportMaterial(entities, _materialIdx, _ingredients, _closingBatch, invoice, sadConsignmentNumber);
        _at = "_exportConsignment";
        CigaretteExportForm _form = GetCigaretteExportForm(batch, invoice, _ingredients, documentName, ref subdocumentNo, invoice.InvoiceIndex.ClearenceIndex.ClearenceProcedure.Value);
        formsList.Add(_form);
      }
      catch (ApplicationError _ar)
      {
        throw _ar;
      }
      catch (Exception _ex)
      {
        string _tmpl = "Cannot proceed with export of Batch: {0} because of error: {1}.";
        throw new ApplicationError("Batch.Export", _at, String.Format(_tmpl, batch.Batch0, _ex.Message), _ex);
      }
    }
    private static void ExportMaterial(Entities entities, Material material, List<Ingredient> formsList, bool closingBatch, InvoiceContent invoiceContent, int sadConsignmentNumber, NamedTraceLogger.TraceAction trace)
    {
      string _at = "Beginning";
      try
      {
        if (material.ProductType.Value == IPR.WebsiteModel.Linq.ProductType.IPRTobacco)
        {
          List<Disposal> _dspsls = new List<Disposal>();
          material.Export(entities, closingBatch, invoiceContent, _dspsls, sadConsignmentNumber, trace);
          foreach (Disposal _dx in _dspsls)
            formsList.Add(GetIPRIngredient(_dx));
        }
        else if (material.ProductType.Value == IPR.WebsiteModel.Linq.ProductType.Tobacco)
        {
          _at = "RegularIngredient";
          RegularIngredient _ri = new RegularIngredient(material.Batch, material.SKU, Convert.ToDouble(material.CalculatedQuantity(invoiceContent)));
          formsList.Add(_ri);
        }
      }
      catch (ApplicationError _ae)
      {
        throw _ae;
      }
      catch (Exception _ex)
      {
        string _tmpl = "Cannot proceed with export of Material: {0} because of error: {1}.";
        throw new ApplicationError("Material.Export", _at, String.Format(_tmpl, material.Material2BatchIndex.Title, _ex.Message), _ex);
      }
    }
    private static void CountExportFormTotals(List<Ingredient> ingredients, CigaretteExportForm form)
    {
      form.Ingredients = ingredients.OrderBy(x =>
      {
        IPRIngredient y = x as IPRIngredient;
        if (y == null)
          return " ";
        else
          return y.DocumentNoumber;
      }).ToArray();
      form.IPTMaterialQuantityTotal = 0;
      form.RegularMaterialQuantityTotal = 0;
      form.IPTDutyVatTotals = new TotalAmountOfMoney();
      foreach (Ingredient _item in ingredients)
      {
        if (_item is IPRIngredient)
        {
          IPRIngredient _iprItem = (IPRIngredient)_item;
          form.IPTMaterialQuantityTotal += _iprItem.TobaccoQuantity;
          form.IPTDutyVatTotals.Add(new AmountOfMoney(_iprItem.TobaccoValue, _iprItem.Duty, _iprItem.VAT, _iprItem.Currency));
        }
        else
        {
          RegularIngredient _rglrItem = (RegularIngredient)_item;
          form.RegularMaterialQuantityTotal += _rglrItem.TobaccoQuantity;
        }
      }
      form.IPTMaterialQuantityTotal = form.IPTMaterialQuantityTotal.RountMass();
      form.IPTDutyVatTotals.AssignTotals();
      form.RegularMaterialQuantityTotal = form.RegularMaterialQuantityTotal.RountMass();
    }
    #endregion
  }
}
