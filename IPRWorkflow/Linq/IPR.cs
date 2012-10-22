using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Customs;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{

  public partial class IPR
  {
    internal enum DisposalEnum { Dust, SHMenthol, Waste, OverusageInKg, Tobacco };
    internal static void CreateIPRAccount
      ( Entities _edc, SADDocumentType _document, Clearence _nc, CustomsDocument.DocumentType _messageType, DateTime _customsDebtDate, out string _comments,
      SADDocumentLib iprLibraryLookup )
    {
      string _at = "started";
      _comments = "IPR account creation error";
      try
      {
        _at = "newIPRData";
        _comments = "Inconsistent or incomplete data to create IPR account";
        IPRData _iprdata = new IPRData( _document, _messageType );
        _at = "Consent.Lookup";
        _comments = "Consent lookup filed";
        Consent _cnsnt = Consent.Lookup( _edc, _iprdata.Consent );
        _at = "PCNCode.AddOrGet";
        _comments = "PCN lookup filed";
        PCNCode _pcn = PCNCode.AddOrGet( _edc, _iprdata.PCNTariffCode );
        _at = "new IPRIPR";
        IPR _ipr = new IPR()
        {
          AccountClosed = false,
          AccountBalance = _iprdata.NetMass,
          Batch = _iprdata.Batch,
          Cartons = _iprdata.Cartons,
          ClearenceIndex = _nc,
          ClosingDate = CAS.SharePoint.Extensions.SPMinimum,
          IPR2ConsentTitle = _cnsnt,
          Currency = _document.Currency,
          CustomsDebtDate = _customsDebtDate,
          DocumentNo = _nc.DocumentNo,
          Duty = _iprdata.Duty,
          DutyName = _iprdata.DutyName,
          IPRDutyPerUnit = _iprdata.DutyPerUnit,
          Grade = _iprdata.GradeName,
          GrossMass = _iprdata.GrossMass,
          InvoiceNo = _iprdata.InvoiceNo,
          IPRLibraryIndex = iprLibraryLookup,
          NetMass = _iprdata.NetMass,
          OGLValidTo = _customsDebtDate + new TimeSpan( Convert.ToInt32( _cnsnt.ConsentPeriod.Value ) * 30, 0, 0, 0 ),
          IPR2PCNPCN = _pcn,
          SKU = _iprdata.SKU,
          TobaccoName = _iprdata.TobaccoName,
          TobaccoNotAllocated = _iprdata.NetMass,
          Title = "-- creating -- ",
          IPRUnitPrice = _iprdata.UnitPrice,
          Value = _iprdata.Value,
          VATName = _iprdata.VATName,
          VAT = _iprdata.VAT,
          IPRVATPerUnit = _iprdata.VATPerUnit
        };
        _at = "new InsertOnSubmit";
        _edc.IPR.InsertOnSubmit( _ipr );
        _at = "new SubmitChanges #1";
        _edc.SubmitChanges();
        _ipr.Title = String.Format( "IPR-{0:D4}{1:D6}", DateTime.Today.Year, _ipr.Identyfikator );
        _at = "new SubmitChanges #2";
        _edc.SubmitChanges();
      }
      catch ( IPRDataConsistencyException _ex )
      {
        _ex.Message.Insert( 0, String.Format( "Message={0}, Reference={1}; ", _messageType, _document.ReferenceNumber ) );
        throw _ex;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "CreateIPRAccount method error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, "IPR account creation error" );
      }
      _comments = "IPR account created";
    }
    internal static List<IPR> FindIPRAccountsWithNotAllocatedTobacco( Entities _edc, string _batch )
    {
      return ( from IPR _iprx in _edc.IPR where _iprx.Batch.Contains( _batch ) && !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0 orderby _iprx.Identyfikator ascending select _iprx ).ToList();
    }
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    internal void AddDisposal( Entities _edc, DisposalEnum _status, ref double quantity, Material material )
    {
      try
      {
        Linq.IPR.DisposalStatus _typeOfDisposal = default( Linq.IPR.DisposalStatus );
        switch ( _status )
        {
          case DisposalEnum.Dust:
            _typeOfDisposal = Linq.IPR.DisposalStatus.Dust;
            break;
          case DisposalEnum.SHMenthol:
            _typeOfDisposal = Linq.IPR.DisposalStatus.SHMenthol;
            break;
          case DisposalEnum.Waste:
            _typeOfDisposal = Linq.IPR.DisposalStatus.Waste;
            break;
          case DisposalEnum.OverusageInKg:
            _typeOfDisposal = Linq.IPR.DisposalStatus.Overuse;
            break;
          case DisposalEnum.Tobacco:
            _typeOfDisposal = Linq.IPR.DisposalStatus.TobaccoInCigaretes;
            break;
        }
        double _toDispose;
        _toDispose = Math.Min( quantity, this.TobaccoNotAllocated.Value );
        this.TobaccoNotAllocated -= _toDispose;
        quantity -= _toDispose;
        Disposal _newDisposal = new Disposal()
        {
          Disposal2BatchIndex = material.Material2BatchIndex,
          Disposal2ClearenceIndex = null,
          ClearingType = Linq.IPR.ClearingType.PartialWindingUp,
          CustomsStatus = Linq.IPR.CustomsStatus.NotStarted,
          CustomsProcedure = String.Empty.NotAvailable(),
          //TODO CompensationGood must be assigned.
          Disposal2PCNID = null,
          PCNCompensationGood = String.Empty.NotAvailable(),
          DisposalStatus = _typeOfDisposal,
          DutyAndVAT = new Nullable<double>(),
          DutyPerSettledAmount = new Nullable<double>(),
          InvoiceNo = String.Empty.NotAvailable(),
          IPRDocumentNo = String.Empty.NotAvailable(), // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
          Disposal2IPRIndex = this,
          VATPerSettledAmount = null,
          JSOXCustomsSummaryIndex = null,
          Disposal2MaterialIndex = material,
          No = new Nullable<double>(),
          // RemainingQuantity = 0,
          SADDate = CAS.SharePoint.Extensions.SPMinimum,
          SADDocumentNo = String.Empty.NotAvailable(),
          SettledQuantity = _toDispose,
          TobaccoValue = _toDispose * this.Value / this.NetMass
        };
        _newDisposal.SetUpCalculatedColumns( ClearingType.PartialWindingUp );
        _edc.Disposal.InsertOnSubmit( _newDisposal );
      }
      catch ( IPRDataConsistencyException _ex )
      {
        throw _ex;
      }
      catch ( Exception _ex )
      {
        string _msg = String.Format
          (
            "Disposal for batch= {0} of type={1} at account=={2} creation failed because of error: " + _ex.Message,
            material.Material2BatchIndex.Title,
            _status,
            this.Title
          );
        throw new IPRDataConsistencyException( "IPR.AddDisposal", _ex.Message, _ex, "Disposal creation failed" );
      }
    }
    private class IPRData
    {
      #region private
      private SADGood FirstSADGood { get; set; }
      private void AnalizeGood( SADDocumentType _document, CustomsDocument.DocumentType _messageType )
      {
        string _at = "Started";
        try
        {
          SADPackage _packagex = FirstSADGood.SADPackage.First();
          _at = "GrossMass";
          if ( _messageType == CustomsDocument.DocumentType.SAD )
            GrossMass = FirstSADGood.GrossMass.HasValue ? FirstSADGood.GrossMass.Value : _document.GrossMass.Value;
          else if ( _messageType == CustomsDocument.DocumentType.PZC )
            GrossMass = _document.GrossMass.HasValue ? _document.GrossMass.Value : FirstSADGood.GrossMass.Value;
          else
            throw new IPRDataConsistencyException( "IPRData.GetCartons", String.Format( "Unexpected message {0} message", _messageType ), null, "Unexpected message" );
          _at = "SADQuantity";
          SADQuantity _quantity = FirstSADGood.SADQuantity.FirstOrDefault();
          NetMass = _quantity == null ? 0 : _quantity.NetMass.GetValueOrDefault( 0 );
          _at = "Cartons";
          if ( _packagex.Package.ToUpper().Contains( "CT" ) )
            Cartons = GrossMass - NetMass;
          else
            Cartons = 0;
        }
        catch ( Exception _ex )
        {
          string _src = String.Format( "AnalizeGood error at {0}", _at );
          throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
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
          foreach ( SADDuties _duty in FirstSADGood.SADDuties )
          {
            _at = "switch " + _duty.DutyType;
            switch ( _duty.DutyType )
            {
              //Duty
              case "A10":
              case "A00":
              case "A20":
                Duty += _duty.Amount.Value;
                _at = "DutyName";
                DutyName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
                break;
              //VAT
              case "B00":
              case "B10":
              case "B20":
                VAT += _duty.Amount.Value;
                _at = "VATName";
                VATName += String.Format( "{0}={1:F2}; ", _duty.DutyType, _duty.Amount.Value );
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
        catch ( Exception _ex )
        {
          string _src = String.Format( "AnalizeDutyAndVAT error at {0}", _at );
          throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
        }
      }
      private const string UnrecognizedName = "-- unrecognized name --";
      private void AnalizeGoodsDescription( string _GoodsDescription )
      {
        string _at = "Started";
        try
        {
          _at = "TobaccoName";
          string _na = "Not recognized";
          TobaccoName = _GoodsDescription.GetFirstCapture( CommonDefinition.GoodsDescriptionTobaccoNamePattern, _na );
          _at = "GradeName";
          GradeName = _GoodsDescription.GetFirstCapture( CommonDefinition.GoodsDescriptionWGRADEPattern, _na );
          _at = "SKU";
          SKU = _GoodsDescription.GetFirstCapture( CommonDefinition.GoodsDescriptionSKUPattern, _na );
          _at = "Batch";
          Batch = _GoodsDescription.GetFirstCapture( CommonDefinition.GoodsDescriptionBatchPattern, _na );
        }
        catch ( Exception _ex )
        {
          string _src = String.Format( "AnalizeGoodsDescription error at {0}", _at );
          throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
        }
      }
      #endregion

      #region cretor
      internal IPRData( SADDocumentType _document, CustomsDocument.DocumentType _messageType )
      {
        string _at = "starting";
        try
        {
          FirstSADGood = _document.SADGood.First();
          AnalizeGood( _document, _messageType );
          AnalizeDutyAndVAT();
          _at = "InvoiceNo";
          this.InvoiceNo = (
                          from _dx in FirstSADGood.SADRequiredDocuments
                          let CustomsProcedureCode = _dx.Code.ToUpper()
                          where CustomsProcedureCode.Contains( "N380" ) || CustomsProcedureCode.Contains( "N935" )
                          select new { Number = _dx.Number }
                       ).First().Number;
          _at = "Consent";
          try
          {
            this.Consent = (
                    from _dx in FirstSADGood.SADRequiredDocuments
                    let CustomsProcedureCode = _dx.Code.ToUpper()
                    where CustomsProcedureCode.Contains( "1PG1" ) || CustomsProcedureCode.Contains( "C601" )
                    select new { Number = _dx.Number }
                 ).First().Number.ToUpper();

          }
          catch ( Exception _ex )
          {
            string _src = String.Format( "IPR.IPRData creator", _at );
            throw new IPRDataConsistencyException( _src, "There is not attached any consent document with code = 1PG1/C601", _ex, _src );
          }
          AnalizeGoodsDescription( FirstSADGood.GoodsDescription );
        }
        catch ( IPRDataConsistencyException es )
        {
          throw es;
        }
        catch ( Exception _ex )
        {
          string _src = String.Format( "IPR.IPRData creator error at {0}", _at );
          throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
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
