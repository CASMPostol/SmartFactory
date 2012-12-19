using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint.Linq;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal static class AccountClearanceFactory
  {
    #region public
    internal static RequestContent CreateRequestContent( IPRClass ipr, string documentNo )
    {
      ProductCodeNumberDesscription[] _pcnArray = CreateArrayOfProductCodeNumberDesscription( ipr.Disposal );
      ArrayOfDIsposalsDisposalsArray[] _disposalsColection = CreateArrayOfDIsposalsDisposalsArray( ipr.Disposal );
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
        ProductivityRateMax = ipr.ProductivityRateMax.Value,
        ProductivityRateMin = ipr.ProductivityRateMin.Value,
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
    private static ProductCodeNumberDesscription[] CreateArrayOfProductCodeNumberDesscription( EntitySet<Disposal> disposals )
    {
      Dictionary<string, ProductCodeNumberDesscription> _ret = new Dictionary<string, ProductCodeNumberDesscription>();
      foreach ( Disposal _dx in disposals.OrderBy<Disposal, double>( x => x.No.Value ) )
      {
        if ( _dx.Disposal2PCNID == null )
          throw new ArgumentNullException( "Disposal2PCNID", "PCN code has to be recognized for all disposals" );
        if ( !_ret.ContainsKey( _dx.Disposal2PCNID.ProductCodeNumber ) )
        {
          ProductCodeNumberDesscription _new = new ProductCodeNumberDesscription()
          {
            CodeNumber = _dx.Disposal2PCNID.ProductCodeNumber,
            Description = _dx.Disposal2PCNID.Title
          };
          _ret.Add( _new.CodeNumber, _new );
        }
      }
      return _ret.Values.OrderBy<ProductCodeNumberDesscription, string>( x => x.CodeNumber ).ToArray<ProductCodeNumberDesscription>();
    }
    private static ArrayOfDIsposalsDisposalsArray[] CreateArrayOfDIsposalsDisposalsArray( EntitySet<Disposal> disposals )
    {
      List<ArrayOfDIsposalsDisposalsArray> _arry = new List<ArrayOfDIsposalsDisposalsArray>();
      foreach ( Disposal _dx in disposals.OrderBy<Disposal, double>( x => x.No.Value ) )
      {
        ArrayOfDIsposalsDisposalsArray _item = new ArrayOfDIsposalsDisposalsArray()
        {
          CustomsProcedure = _dx.CustomsProcedure,
          DutyAndVAT = _dx.DutyAndVAT.GetValueOrDefault( 0 ),
          DutyPerSettledAmount = _dx.DutyPerSettledAmount.GetValueOrDefault( 0 ),
          InvoiceNo = _dx.InvoiceNo,
          No = _dx.No.Value,
          ProductCodeNumber = _dx.Disposal2PCNID == null ? String.Empty.NotAvailable() : _dx.Disposal2PCNID.ProductCodeNumber,
          RemainingQuantity = _dx.RemainingQuantity.Value,
          SADDate = _dx.SADDate.Value,
          SADDocumentNo = _dx.SADDocumentNo,
          SettledQuantity = _dx.SettledQuantity.Value,
          VATPerSettledAmount = _dx.VATPerSettledAmount.GetValueOrDefault( 0 )
        };
        _arry.Add( _item );
      }
      return _arry.ToArray();
    }
    #endregion

  }
}

