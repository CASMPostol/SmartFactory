using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class IPR
  {
    internal static void CreateIPRAccount(EntitiesDataContext _edc, SADDocumentType _document, Clearence _nc)
    {
      Consent _consent = Consent.Lookup(_edc, _document.GetConsentNo());
      IPRData _iprdata = new IPRData(_document);
      IPR _ipr = new IPR()
      {
        Cartons = _iprdata.Cartons,
        ClearenceListLookup = _nc,
        ClosingDate = new Nullable<DateTime>(),
        ConsentLookup = _consent,
        ConsentNo = default(string),  //TODO replace by secondary lookup  http://itrserver/Bugs/BugDetail.aspx?bid=3392
        Currency = _document.Currency,
        CustomsDebtDate = _document.CustomsDebtDate,
        DocumentNo = _nc.DocumentNo,
        Duty = _iprdata.Duty,
        DutyName = _iprdata.DutyName,
        DutyPerUnit = _iprdata.DutyPerUnit,
        GrossMass = _iprdata.GrossMass,
        InvoiceNo = _iprdata.InvoiceNo,
        NetMass = _iprdata.NetMass,
        No = default(double), //TODO  [pr4-3394] Not clear how to evaluate some IPR colummns http://itrserver/Bugs/BugDetail.aspx?bid=3394
        OGLValidTo = _document.CustomsDebtDate.Value + new TimeSpan(Convert.ToInt32(_consent.ConsentPeriod.Value) * 30, 0, 0),
        ProductivityRateMax = _consent.ProductivityRateMax,
        ProductivityRateMin = _consent.ProductivityRateMax,
        TobaccoName = _iprdata.TobaccoName,
        Tytuł = _iprdata.Title,
        UnitPrice = _iprdata.UnitPrice,
        Value = _iprdata.Value,
        VATName = _iprdata.VATName,
        VAT = _iprdata.VAT,
        VATPerUnit = _iprdata.VATPerUnit
      };
      _edc.IPR.InsertOnSubmit(_ipr);
      _edc.SubmitChanges();
    }
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    internal class IPRData
    {
      private SADDocumentType _document;
      public IPRData(SADDocumentType _document)
      {
        // TODO: Complete member initialization
        this._document = _document;
      }
      public double Cartons { get; private set; }
      public double Duty { get; private set; } // TODO   http://itrserver/Bugs/BugDetail.aspx?bid=3393
      public string DutyName { get; private set; }
      public double DutyPerUnit { get; private set; }
      public double GrossMass { get; private set; }
      public string InvoiceNo { get; private set; }
      public double NetMass { get; private set; }
      public string TobaccoName { get; private set; }
      public string Title { get; private set; }
      public double UnitPrice { get; private set; }
      public double Value { get; private set; }
      public string VATName { get; private set; }
      public double VAT { get; private set; }
      public double VATPerUnit { get; private set; }
    }
  }
}
