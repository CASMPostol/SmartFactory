using System;
using System.Linq;
using CAS.SmartFactory.xml.Customs;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class IPR
  {
    internal static void CreateIPRAccount(EntitiesDataContext _edc, SADDocumentType _document, Clearence _nc, CustomsDocument.DocumentType _messageType, DateTime _customsDebtDate, out string _comments)
    {
      string _at = "started";
      _comments = "IPR account creation error";
      try
      {
        _at = "newIPRData";
        _comments = "Inconsistent or incomplete data to create IPR account";
        IPRData _iprdata = new IPRData(_document, _messageType);
        _at = "Consent.Lookup";
        _comments = "Consent lookup filed";
        Consent _cnsnt = Consent.Lookup(_edc, _iprdata.Consent);
        _at = "PCNCode.AddOrGet";
        _comments = "PCN lookup filed";
        PCNCode _pcn = PCNCode.AddOrGet(_edc, _iprdata.PCNTariffCode);
        _at = "new IPRIPR";
        IPR _ipr = new IPR()
        {
          AccountClosed = false,
          AccountBalance = _iprdata.NetMass,
          Batch = _iprdata.Batch,
          Cartons = _iprdata.Cartons,
          ClearenceListLookup = _nc,
          //ClosingDate = new Nullable<DateTime>(),
          ConsentNo = _cnsnt,
          Currency = _document.Currency,
          CustomsDebtDate = _customsDebtDate,
          DocumentNo = _nc.DocumentNo,
          Duty = _iprdata.Duty,
          DutyName = _iprdata.DutyName,
          DutyPerUnit = _iprdata.DutyPerUnit,
          Grade = _iprdata.GradeName,
          GrossMass = _iprdata.GrossMass,
          InvoiceNo = _iprdata.InvoiceNo,
          NetMass = _iprdata.NetMass,
          OGLValidTo = _customsDebtDate + new TimeSpan(Convert.ToInt32(_cnsnt.ConsentPeriod.Value) * 30, 0, 0, 0),
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
        _ex.Message.Insert(0, String.Format("Message={0}, Reference={1}; ", _messageType, _document.ReferenceNumber));
        throw _ex;
      }
      catch (Exception _ex)
      {
        string _src = String.Format("CreateIPRAccount method error at {0}", _at);
        throw new IPRDataConsistencyException(_src, _ex.Message, _ex, "IPR account creation error");
      }
      _comments = "IPR account created";
    }
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    internal enum DisposalEnum { Dust, SHMenthol, Waste, OverusageInKg, Tobacco };
    internal void AddDisposal(EntitiesDataContext _edc, KeyValuePair<DisposalEnum, double> _item, Batch _batch)
    {
      try
      {
        Entities.DisposalStatus _typeOfDisposal = default(Entities.DisposalStatus);
        switch (_item.Key)
        {
          case DisposalEnum.Dust:
            _typeOfDisposal = Entities.DisposalStatus.Dust;
            break;
          case DisposalEnum.SHMenthol:
            _typeOfDisposal = Entities.DisposalStatus.SHMenthol;
            break;
          case DisposalEnum.Waste:
            _typeOfDisposal = Entities.DisposalStatus.Waste;
            break;
          case DisposalEnum.OverusageInKg:
            _typeOfDisposal = Entities.DisposalStatus.TobaccoInCigaretesDestinationEU;
            break;
          case DisposalEnum.Tobacco:
            _typeOfDisposal = Entities.DisposalStatus.TobaccoInCigaretesExported;
            break;
        }
        int _position = 0; //TODO this.DisposalDisposal.Count(); [pr4-3437] No reverse lookup from IPR to Disposal http://itrserver/Bugs/BugDetail.aspx?bid=3437
        DisposalDisposal _newDisposal = new DisposalDisposal()
        {
          BatchLookup = _batch,
          ClearenceListLookup = null,
          ClearingType = Entities.ClearingType.PartialWindingUp,
          CustomsStatus = Entities.CustomsStatus.NotStarted,
          CustomsProcedure = "N/A",
          DisposalStatus = _typeOfDisposal,
          DutyAndVAT = new Nullable<double>(),
          DutyPerSettledAmount = new Nullable<double>(),
          InvoiceNo = "N/A",
          IPRDocumentNo = "N/A", // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
          IPRID = this,
          CompensationGood = default(Entities.PCNCode),
          VATPerSettledAmount = null,
          JSOXCustomsSummaryListLookup = null,
          No = _position,
          RemainingQuantity = 0,
          SADDate = new Nullable<DateTime>(),
          SADDocumentNo = "N/A",
          SettledQuantity = _item.Value,
          TobaccoValue = _item.Value * this.Value / this.NetMass
        };
        _edc.Disposal.InsertOnSubmit(_newDisposal);
      }
      catch (Exception _ex)
      {
        string _msg = String.Format
          (
            "Disposal for batch= {0} of type={1} at account=={2} creation failed because of error: " + _ex.Message,
            _batch.Tytuł,
            _item.Key,
            this.Tytuł
          );
        throw new IPRDataConsistencyException("IPR.AddDisposal", _ex.Message, _ex, "Disposal creation failed");
      }
    }
    internal static IPR FindIPRAccount(EntitiesDataContext _edc, string _batch, double _requested)
    {
      try
      {
        return (from IPR _iprx in _edc.IPR where (!_iprx.AccountClosed.Value && _iprx.Batch.Contains(_batch) && _iprx.AccountBalance >= _requested) orderby _iprx.Identyfikator descending select _iprx).First<IPR>();
      }
      catch (Exception ex)
      {
        string _mssg = "Cannot find any IPR account to dispose the tobacco: {0} kg, batch:{1}";
        throw new IPRDataConsistencyException("Material.FindIPRAccount", String.Format(_mssg, _requested, _batch), ex, "IPR unrecognized account");
      }
    }
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
            _at = "switch " + _duty.DutyType;
            switch (_duty.DutyType)
            {
              //Duty
              case "A10":
              case "A00":
              case "A20":
                Duty += _duty.Amount.Value;
                _at = "DutyName";
                DutyName += String.Format("{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value);
                break;
              //VAT
              case "B00":
              case "B10":
              case "B20":
                VAT += _duty.Amount.Value;
                _at = "VATName";
                VATName += String.Format("{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value);
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
      private const string UnrecognizedName = "-- unrecognized name --";
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
          try
          {
            this.Consent = (
                    from _dx in FirstSADGood.SADRequiredDocuments
                    let CustomsProcedureCode = _dx.Code.ToUpper()
                    where CustomsProcedureCode.Contains("1PG1") || CustomsProcedureCode.Contains("C601")
                    select new { Number = _dx.Number }
                 ).First().Number.ToUpper();

          }
          catch (Exception _ex)
          {
            string _src = String.Format("IPR.IPRData creator", _at);
            throw new IPRDataConsistencyException(_src, "There is not attached any consent document with code = 1PG1/C601", _ex, _src);
          } 
          AnalizeGoodsDescription(FirstSADGood.GoodsDescription);
        }
        catch (IPRDataConsistencyException es)
        {
          throw es;
        }
        catch (Exception _ex)
        {
          string _src = String.Format("IPR.IPRData creator error at {0}", _at);
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
