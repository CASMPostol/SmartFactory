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
      string _at = "started";
      try
      {
        _at = "newIPRData";
        IPRData _iprdata = new IPRData(_document, _messageType);
        _at = "Consent.Lookup";
        Consent _cnsnt = Consent.Lookup(_edc, _iprdata.Consent);
        _at = "PCNCode.AddOrGet";
        PCNCode _pcn = PCNCode.AddOrGet(_edc, _iprdata.PCNTariffCode);
        _at = "new IPRIPR";
        IPRIPR _ipr = new IPRIPR()
        {
          Batch = _iprdata.Batch,
          Cartons = _iprdata.Cartons,
          ClearenceListLookup = _nc,
          ClosingDate = new Nullable<DateTime>(),
          ConsentNo = _cnsnt,
          Currency = _document.Currency,
          CustomsDebtDate = _document.CustomsDebtDate,
          DocumentNo = _nc.DocumentNo,
          Duty = _iprdata.Duty,
          DutyName = _iprdata.DutyName,
          DutyPerUnit = _iprdata.DutyPerUnit,
          //TODO GradeName = _iprdata.GradeName, - (old name:"type") column must be added.
          GrossMass = _iprdata.GrossMass,
          InvoiceNo = _iprdata.InvoiceNo,
          NetMass = _iprdata.NetMass,
          No = 1,
          OGLValidTo = _document.CustomsDebtDate.Value + new TimeSpan(Convert.ToInt32(_cnsnt.ConsentPeriod.Value) * 30, 0, 0),
          PCNTariffCode = _iprdata.PCNTariffCode, //TODO - intialize column from GoodsDescription [pr4-3398] List: IPR - columns should be updated http://itrserver/Bugs/BugDetail.aspx?bid=3398
          SKU = _iprdata.SKU,
          TobaccoName = _iprdata.TobaccoName,
          Tytuł = "-- creating -- ",
          UnitPrice = _iprdata.UnitPrice,
          Value = _iprdata.Value,
          VATName = _iprdata.VATName,
          VAT = _iprdata.VAT,
          VATPerUnit = _iprdata.VATPerUnit
        };
        _at = "new InsertOnSubmit";
        _edc.IPR.InsertOnSubmit(_ipr);
        _at = "new SubmitChanges #1";
        _edc.SubmitChanges();
        _ipr.Tytuł = String.Format("IPR-{0:D4}{1:D6}", DateTime.Today.Year, _ipr.Identyfikator);
        _at = "new SubmitChanges #2";
        _edc.SubmitChanges();
      }
      catch (IPRDataConsistencyException _ex)
      {
        throw _ex;
      }
      catch (Exception _ex)
      {
        string _src = String.Format("CreateIPRAccount method error at {0}", _at);
        throw new IPRDataConsistencyException(_src, _ex.Message, _ex);
      }
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
        string _at = "Started";
        try
        {
          SADPackage _packagex = FirstSADGood.SADPackage.First();
          _at = "GrossMass";
          if (_messageType == CustomsDocument.DocumentType.SAD)
            GrossMass = FirstSADGood.GrossMass.HasValue ? FirstSADGood.GrossMass.Value : _document.GrossMass.Value;
          else if (_messageType == CustomsDocument.DocumentType.PZC)
            GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : FirstSADGood.GrossMass.Value;
          else
            throw new IPRDataConsistencyException("IPRData.GetCartons", String.Format("Unexpected message {0} message", _messageType), null);
          _at = "SADQuantity";
          SADQuantity _quantity = FirstSADGood.SADQuantity.First();
          _at = "NetMass";
          NetMass = _quantity.NetMass.Value;
          _at = "Cartons";
          if (_packagex.Package.ToUpper().Contains("CT"))
            Cartons = GrossMass - NetMass;
          else
            Cartons = 0;
        }
        catch (Exception _ex)
        {
          string _src = String.Format("AnalizeGood error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex);
        }
      }
      private void AnalizeDutyAndVAT()
      {
        string _at = "Started";
        try
        {
          Duty = 0;
          VAT = 0;
          DutyName = string.Empty;
          VATName = string.Empty;
          foreach (SADDuties _duty in FirstSADGood.SADDuties)
          {
            _at = "switch " + _duty.Tytuł;
            switch (_duty.Tytuł)
            {
              //Duty
              case "A10":
              case "A00":
              case "A20":
                Duty += _duty.Amount.Value;
                _at = "DutyName";
                DutyName += String.Format("{0}={1:F2}; ", _duty.Tytuł, _duty.Amount.Value);
                break;
              //VAT
              case "B00":
              case "B10":
              case "B20":
                VAT += _duty.Amount.Value;
                _at = "VATName";
                VATName += String.Format("{0}={1:F2}; ", _duty.Tytuł, _duty.Amount.Value);
                break;
              default:
                break;
            }
          }
          _at = "DutyPerUnit";
          DutyPerUnit = Duty / NetMass;
          _at = "VATPerUnit";
          VATPerUnit = VAT / NetMass;
          _at = "Value";
          Value = FirstSADGood.TotalAmountInvoiced.Value;
          _at = "UnitPrice";
          UnitPrice = Value / NetMass;
        }
        catch (Exception _ex)
        {
          string _src = String.Format("AnalizeDutyAndVAT error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex);
        }
      }
      private void AnalizeGoodsDescription()
      {
        string _at = "Started";
        try
        {
          _at = "TobaccoName";
          Match _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"\b(.*)\s+(?=GRADE:)", RegexOptions.IgnoreCase);
          const string UnrecognizedName = "-- unrecognized name --";
          if (_tnm.Success && _tnm.Groups.Count == 1)
            TobaccoName = _tnm.Captures[1].Value;
          else
            TobaccoName = UnrecognizedName;
          _at = "GradeName";
          _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WGRADE:)\W*\b(\w*)", RegexOptions.IgnoreCase);
          if (_tnm.Success && _tnm.Groups.Count == 1)
            GradeName = _tnm.Captures[1].Value;
          else
            GradeName = UnrecognizedName;
          _at = "SKU";
          _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WSKU:)\W*\b(\d*)", RegexOptions.IgnoreCase);
          if (_tnm.Success && _tnm.Groups.Count == 1)
            SKU = _tnm.Captures[1].Value;
          else
            SKU = UnrecognizedName;
          _at = "Batch";
          _tnm = Regex.Match(FirstSADGood.GoodsDescription, @"(?<=\WBatch:)\W*\b(\d*)", RegexOptions.IgnoreCase);
          if (_tnm.Success && _tnm.Groups.Count == 1)
            Batch = _tnm.Captures[1].Value;
          else
            Batch = UnrecognizedName;

        }
        catch (Exception _ex)
        {
          string _src = String.Format("AnalizeDutyAndVAT error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex);
        }
      }
      #endregion

      #region cretor
      internal IPRData(SADDocumentType _document, CustomsDocument.DocumentType _messageType)
      {
        string _at = "starting";
        try
        {
          FirstSADGood = _document.SADGood.First();
          AnalizeGood(_document, _messageType);
          AnalizeDutyAndVAT();
          _at = "InvoiceNo";
          this.InvoiceNo = (
                          from _dx in FirstSADGood.SADRequiredDocuments
                          let CustomsProcedureCode = _dx.Code.ToUpper()
                          where CustomsProcedureCode.Contains("N380") || CustomsProcedureCode.Contains("N935")
                          select new { Number = _dx.Number }
                       ).First().Number;
          _at = "Consent";
          this.Consent = (
                          from _dx in FirstSADGood.SADRequiredDocuments
                          let CustomsProcedureCode = _dx.Code.ToUpper()
                          where CustomsProcedureCode.Contains("1PG1") || CustomsProcedureCode.Contains("C601")
                          select new { Number = _dx.Number }
                       ).First().Number;
          AnalizeGoodsDescription();

        }
        catch (IPRDataConsistencyException es)
        {
          throw es;
        }
        catch (Exception _ex)
        {
          string _src = String.Format("IPRData creator error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex);
        }
      }
      #endregion

      #region public
      internal double Cartons { get; private set; }
      internal string Consent { get; private set; }
      internal double Duty { get; private set; }
      internal string DutyName { get; private set; }
      internal double DutyPerUnit { get; private set; }
      internal string GradeName { get; private set; }
      internal double GrossMass { get; private set; }
      internal string InvoiceNo { get; private set; }
      internal double NetMass { get; private set; }
      internal string TobaccoName { get; private set; }
      internal double UnitPrice { get; private set; }
      internal double Value { get; private set; }
      internal string VATName { get; private set; }
      internal double VAT { get; private set; }
      internal double VATPerUnit { get; private set; }
      internal string Batch { get; private set; }
      internal string PCNTariffCode { get { return FirstSADGood.PCNTariffCode; } }
      internal string SKU { get; private set; }
      #endregion
    }
  }
}
