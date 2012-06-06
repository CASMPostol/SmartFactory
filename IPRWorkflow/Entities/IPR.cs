using System;
using System.Linq;
using System.Text.RegularExpressions;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class IPR
  {
    internal static void CreateIPRAccount(EntitiesDataContext _edc, SADDocumentType _document, Clearence _nc, CustomsDocument.DocumentType _messageType)
    {
      Consent _consent = Consent.Lookup(_edc, _document.GetConsentNo());
      IPRData _iprdata = new IPRData(_document, _messageType);
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
        No = 1,
        OGLValidTo = _document.CustomsDebtDate.Value + new TimeSpan(Convert.ToInt32(_consent.ConsentPeriod.Value) * 30, 0, 0),
        ProductivityRateMax = _consent.ProductivityRateMax,
        ProductivityRateMin = _consent.ProductivityRateMax,
        TobaccoName = _iprdata.TobaccoName,
        Tytuł = "-- creating -- ",
        UnitPrice = _iprdata.UnitPrice,
        Value = _iprdata.Value,
        VATName = _iprdata.VATName,
        VAT = _iprdata.VAT,
        VATPerUnit = _iprdata.VATPerUnit
      };
      _edc.IPR.InsertOnSubmit(_ipr);
      _edc.SubmitChanges();
      _ipr.Tytuł = String.Format("IPR-{0:D4}{1:D6}", DateTime.Today.Year, _ipr.Identyfikator);
      _edc.SubmitChanges();
    }
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    private class IPRData
    {
      #region private
      private SADGood FirstSADGood { get; set; }
      private void AnalizeGood(SADDocumentType _document, CustomsDocument.DocumentType _messageType)
      {
        SADPackage _packagex = FirstSADGood.SADPackage.First();
        if (_messageType == CustomsDocument.DocumentType.SAD)
          GrossMass = FirstSADGood.GrossMass.HasValue ? FirstSADGood.GrossMass.Value : _document.GrossMass.Value;
        else if (_messageType == CustomsDocument.DocumentType.PZC)
          GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : FirstSADGood.GrossMass.Value;
        else
          throw new CustomsDataException("IPRData.GetCartons", String.Format("Unexpected message {0} message", _messageType));
        SADQuantity _quantity = FirstSADGood.SADQuantity.First();
        NetMass = _quantity.NetMass.Value;
        if (_packagex.Package.ToUpper().Contains("CT"))
          Cartons = GrossMass - NetMass;
        else
          Cartons = 0;
      }
      private void AnalizeDutyAndVAT()
      {
        Duty = 0;
        VAT = 0;
        DutyName = string.Empty;
        VATName = string.Empty;
        foreach (SADDuties _duty in FirstSADGood.SADDuties)
        {
          switch (_duty.Tytuł)
          {
            //Duty
            case "A10":
            case "A00":
            case "A20":
              Duty += _duty.Amount.Value;
              DutyName += String.Format("{0}={1:F2}; ", _duty.Tytuł, _duty.Amount.Value);
              break;
            //VAT
            case "B00":
            case "B10":
            case "B20":
              VAT += _duty.Amount.Value;
              VATName += String.Format("{0}={1:F2}; ", _duty.Tytuł, _duty.Amount.Value);
              break;
            default:
              break;
          }
        }
        DutyPerUnit = Duty / NetMass;
        VATPerUnit = VAT / NetMass;
        Value = FirstSADGood.TotalAmountInvoiced.Value;
        UnitPrice = Value / NetMass;
      }
      private void AnalizeGoodsDescription()
      {
        Match _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"\b(.*)\s+(?=GRADE:)", RegexOptions.IgnoreCase);
        const string UnrecognizedName = "-- unrecognized name --";
        if (_tnm.Success && _tnm.Groups.Count == 1)
          TobaccoName = _tnm.Captures[1].Value;
        else
          TobaccoName = UnrecognizedName;
        //TODO - intialize column from GoodsDescription [pr4-3398] List: IPR - columns should be updated http://itrserver/Bugs/BugDetail.aspx?bid=3398
        _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WGRADE:)\W*\b(\w*)", RegexOptions.IgnoreCase);
        string GradeName = String.Empty;
        if (_tnm.Success && _tnm.Groups.Count == 1)
          GradeName = _tnm.Captures[1].Value;
        else
          TobaccoName = UnrecognizedName;
        _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WSKU:)\W*\b(\d*)", RegexOptions.IgnoreCase);
        string SKU = String.Empty;
        if (_tnm.Success && _tnm.Groups.Count == 1)
          SKU = _tnm.Captures[1].Value;
        else
          SKU = UnrecognizedName;
        _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WBatch:)\W*\b(\d*)", RegexOptions.IgnoreCase);
        string Batch = String.Empty;
        if (_tnm.Success && _tnm.Groups.Count == 1)
          Batch = _tnm.Captures[1].Value;
        else
          Batch = UnrecognizedName;
      }
      #endregion

      #region cretor
      public IPRData(SADDocumentType _document, CustomsDocument.DocumentType _messageType)
      {
        FirstSADGood = _document.SADGood.First();
        AnalizeGood(_document, _messageType);
        AnalizeDutyAndVAT();
        InvoiceNo = (
                        from _dx in FirstSADGood.SADRequiredDocuments
                        let CustomsProcedureCode = _dx.Code.ToUpper()
                        where CustomsProcedureCode.Contains("N380") || CustomsProcedureCode.Contains("N935")
                        select new { Number = _dx.Number }
                     ).First().Number;
        AnalizeGoodsDescription();
      }
      #endregion

      #region public
      public double Cartons { get; private set; }
      public double Duty { get; private set; }
      public string DutyName { get; private set; }
      public double DutyPerUnit { get; private set; }
      public double GrossMass { get; private set; }
      public string InvoiceNo { get; private set; }
      public double NetMass { get; private set; }
      public string TobaccoName { get; private set; }
      public double UnitPrice { get; private set; }
      public double Value { get; private set; }
      public string VATName { get; private set; }
      public double VAT { get; private set; }
      public double VATPerUnit { get; private set; }
      #endregion
    }
  }
}
