//<summary>
//  Title   : class AccountClearanceFactory
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

using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal static class AccountClearanceFactory
  {
    #region public
    internal static RequestContent CreateRequestContent(List<Disposal> _Disposals, IPRClass ipr, string documentName)
    {
      int documentNo = ipr.Id.Value;
      ProductCodeNumberDesscription[] _pcnArray = CreateArrayOfProductCodeNumberDesscription(_Disposals);
      ArrayOfDIsposalsDisposalsArray[] _disposalsColection = CreateArrayOfDIsposalsDisposalsArray(_Disposals);
      RequestContent _ret = new RequestContent()
      {
        Batch = ipr.Batch,
        Cartons = ipr.Cartons.Value,
        ConsentDate = ipr.IPR2ConsentTitle.ValidFromDate.Value,
        ConsentNo = ipr.IPR2ConsentTitle.Title,
        ConsentPeriod = ipr.ConsentPeriod.Value,
        CustomsDebtDate = ipr.CustomsDebtDate.Value,
        DisposalsColection = _disposalsColection,
        DocumentDate = DateTime.Today.Date,
        DocumentName = documentName,
        DocumentNo = documentNo,
        Duty = ipr.Duty.Value,
        DutyName = ipr.DutyName,
        DutyPerUnit = ipr.IPRDutyPerUnit.Value,
        EntryDocumentNo = ipr.DocumentNo,
        Grade = ipr.Grade,
        GrossMass = ipr.GrossMass.Value,
        InvoiceNo = ipr.InvoiceNo,
        NetMass = ipr.NetMass.Value,
        PCN = ipr.IPR2PCNPCN.ProductCodeNumber,
        PCNRecord = _pcnArray,
        ProductivityRateMax = ipr.ProductivityRateMax.GetValueOrDefault(-1),
        ProductivityRateMin = ipr.ProductivityRateMin.GetValueOrDefault(-1),
        SKU = ipr.SKU,
        TobaccoName = ipr.TobaccoName,
        ValidFromDate = ipr.ValidFromDate.Value,
        ValidToDate = ipr.ValidToDate.Value,
        VAT = ipr.VAT.Value,
        VATName = ipr.VATName,
        VATPerUnit = ipr.IPRVATPerUnit.Value,
        VATDutyTotal = ipr.VATDec + ipr.DutyDec
      };
      return _ret;
    }
    #endregion

    #region private
    private static ProductCodeNumberDesscription[] CreateArrayOfProductCodeNumberDesscription(IEnumerable<Disposal> disposals)
    {
      Dictionary<string, ProductCodeNumberDesscription> _ret = new Dictionary<string, ProductCodeNumberDesscription>();
      foreach (Disposal _dx in disposals.OrderBy<Disposal, double>(x => x.SPNo.HasValue ? x.SPNo.Value : x.Created.Value.Ticks))
      {
        if (!_ret.ContainsKey(_dx.Disposal2PCNID.ProductCodeNumber))
        {
          ProductCodeNumberDesscription _new = new ProductCodeNumberDesscription()
          {
            CodeNumber = _dx.Disposal2PCNID.ProductCodeNumber,
            Description = _dx.Disposal2PCNID.Title
          };
          _ret.Add(_new.CodeNumber, _new);
        }
      }
      return _ret.Values.OrderBy<ProductCodeNumberDesscription, string>(x => x.CodeNumber).ToArray<ProductCodeNumberDesscription>();
    }
    private static ArrayOfDIsposalsDisposalsArray[] CreateArrayOfDIsposalsDisposalsArray(IEnumerable<Disposal> disposals)
    {
      List<ArrayOfDIsposalsDisposalsArray> _arry = new List<ArrayOfDIsposalsDisposalsArray>();
      foreach (Disposal _dx in disposals.Where(x => x.SettledQuantityDec > 0).OrderBy<Disposal, double>(x => x.SPNo.HasValue ? x.SPNo.Value : x.Created.Value.Ticks))
      {
        ArrayOfDIsposalsDisposalsArray _item = new ArrayOfDIsposalsDisposalsArray()
        {
          CustomsProcedure = _dx.CustomsProcedure,
          DutyAndVAT = _dx.DutyAndVAT.GetValueOrDefault(0),
          DutyPerSettledAmount = _dx.DutyPerSettledAmount.GetValueOrDefault(0),
          InvoiceNo = _dx.InvoiceNo,
          No = _dx.SPNo.GetValueOrDefault(-1),
          ProductCodeNumber = _dx.Disposal2PCNID == null ? String.Empty.NotAvailable() : _dx.Disposal2PCNID.ProductCodeNumber,
          RemainingQuantity = _dx.RemainingQuantity.GetValueOrDefault(-1),
          SadConsignmentNo = _dx.SadConsignmentNo,
          SADDate = _dx.SADDate.GetValueOrNull(),
          SADDocumentNo = _dx.SADDocumentNo,
          SettledQuantity = _dx.SettledQuantity.Value,
          VATPerSettledAmount = _dx.VATPerSettledAmount.GetValueOrDefault(0)
        };
        _arry.Add(_item);
      }
      return _arry.ToArray();
    }
    #endregion

  }
}

