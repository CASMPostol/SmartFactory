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
        IPR _ipr = new IPR()
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
          Grade = _iprdata.GradeName, 
          GrossMass = _iprdata.GrossMass,
          InvoiceNo = _iprdata.InvoiceNo,
          NetMass = _iprdata.NetMass,
          No = 1,
          //TODO [pr4-3408] Calculation of the OGLValidTo column http://itrserver/Bugs/BugDetail.aspx?bid=3408
          OGLValidTo = _document.CustomsDebtDate.Value + new TimeSpan(Convert.ToInt32(_cnsnt.ConsentPeriod.Value) * 30, 0, 0, 0), 
          PCNTariffCode = _pcn,
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
        throw new IPRDataConsistencyException(_src, _ex.Message, _ex, "IPR account creation error");
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
            throw new IPRDataConsistencyException("IPRData.GetCartons", String.Format("Unexpected message {0} message", _messageType), null, "Unexpected message");
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
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex, _src);
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
            _at = "switch " + _duty.Type;
            switch (_duty.Type)
            {
              //Duty
              case "A10":
              case "A00":
              case "A20":
                Duty += _duty.Amount.Value;
                _at = "DutyName";
                DutyName += String.Format("{0}={1:F2}; ", _duty.Type, _duty.Amount.Value);
                break;
              //VAT
              case "B00":
              case "B10":
              case "B20":
                VAT += _duty.Amount.Value;
                _at = "VATName";
                VATName += String.Format("{0}={1:F2}; ", _duty.Type, _duty.Amount.Value);
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
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex, _src);
        }
      }
      const string UnrecognizedName = "-- unrecognized name --";
      private void AnalizeGoodsDescription(string _GoodsDescription)
      {
        string _at = "Started";
        try
        {
          _at = "TobaccoName";
          TobaccoName = _GoodsDescription.GetFirstCapture(CommonDefinition.GoodsDescriptionTobaccoNamePattern);
          _at = "GradeName";
          GradeName = _GoodsDescription.GetFirstCapture(@"(?<=\WGRADE:)\W*\b(\w*)");
          _at = "SKU";
          SKU = _GoodsDescription.GetFirstCapture(@"(?<=\WSKU:)\W*\b(\d*)");
          _at = "Batch";
          Batch = _GoodsDescription.GetFirstCapture(@"(?<=\WBatch:)\W*\b(\d*)");
        }
        catch (Exception _ex)
        {
          string _src = String.Format("AnalizeGoodsDescription error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex, _src);
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
          AnalizeGoodsDescription(FirstSADGood.GoodsDescription);

        }
        catch (IPRDataConsistencyException es)
        {
          throw es;
        }
        catch (Exception _ex)
        {
          string _src = String.Format("IPRData creator error at {0}", _at);
          throw new IPRDataConsistencyException(_src, _ex.Message, _ex, _src);
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
