﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint.Linq;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal static class AccountClearanceFactory
  {
    public static RequestContent CreateRequestContent( IPRClass ipr, string documentNo )
    {
      ArrayOfDIsposalsDisposalsArray[] _disposalsColection = CreateArrayOfDIsposalsDisposalsArray( ipr.Disposal );
      RequestContent _ret = new RequestContent()
      {
        Batch = ipr.Batch,
        Cartons = ipr.Cartons.Value,
        ConsentDate = ipr.IPR2ConsentTitle.ConsentDate.Value,
        ConsentNo = ipr.IPR2ConsentTitle.Title,
        ConsentPeriod = ipr.ConsentPeriod.Value,
        CustomsDebtDate = ipr.CustomsDebtDate.Value,
        DisposalsColection = _disposalsColection,
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentNo,
        Duty = ipr.Duty.Value,
        DutyName = ipr.DutyName,
        DutyPerUnit = ipr.DutyPerUnit,
        EntryDocumentNo = ipr.DocumentNo,
        Grade = ipr.Grade,
        GrossMass = ipr.GrossMass.Value,
        InvoiceNo = ipr.InvoiceNo,
        NetMass = ipr.NetMass.Value,
        PCN = ipr.IPR2PCNPCN.ProductCodeNumber,
        ProductivityRateMax = ipr.ProductivityRateMax.Value,
        ProductivityRateMin = ipr.ProductivityRateMin.Value,
        SKU = ipr.SKU,
        TobaccoName = ipr.TobaccoName,
        ValidFromDate = ipr.ValidFromDate.Value,
        ValidToDate = ipr.ValidToDate.Value,
        VAT = ipr.VAT.Value,
        VATName = ipr.VATName,
        VATPerUnit = ipr.VATPerUnit
      };
      return _ret;
    }
    private static ArrayOfDIsposalsDisposalsArray[] CreateArrayOfDIsposalsDisposalsArray( EntitySet<Disposal> disposals )
    {
      List<ArrayOfDIsposalsDisposalsArray> _arry = new List<ArrayOfDIsposalsDisposalsArray>();
      foreach ( Disposal _dx in disposals )
      {
        ArrayOfDIsposalsDisposalsArray _item = new ArrayOfDIsposalsDisposalsArray()
        {
          CustomsProcedure = _dx.CustomsProcedure,
          DutyAndVAT = _dx.DutyAndVAT.Value,
          DutyPerSettledAmount = _dx.DutyPerSettledAmount.Value,
          InvoiceNo = _dx.InvoiceNo,
          No = _dx.No.Value,
          ProductCodeNumber = _dx.Disposal2PCNID.ProductCodeNumber,
          RemainingQuantity = _dx.RemainingQuantity.Value,
          SADDate = _dx.SADDate.Value,
          SADDocumentNo = _dx.SADDocumentNo,
          SettledQuantity = _dx.SettledQuantity.Value,
          VATPerSettledAmount = _dx.VATPerSettledAmount.Value
        };
        _arry.Add( _item );
      }
      return _arry.ToArray();
    }
  }
}

