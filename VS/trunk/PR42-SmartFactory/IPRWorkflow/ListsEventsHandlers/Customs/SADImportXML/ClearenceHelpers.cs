using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.Customs
{
  internal static class ClearenceHelpers
  {

    #region MyRegion
    internal static IQueryable<Clearence> Associate( Entities _edc, CustomsDocument.DocumentType _messageType, SADDocumentType _sad, out string _comments, SADDocumentLib customsDocumentLibrary )
    {
      IQueryable<Clearence> _ret = default( IQueryable<Clearence> );
      string _at = "started";
      _comments = "Clearance association error";
      try
      {
        //TODO common naming mechanism must implemented
        switch ( _messageType )
        {
          case CustomsDocument.DocumentType.SAD:
          case CustomsDocument.DocumentType.PZC:
            _at = "_customsProcedureCodes";
            CustomsProcedureCodes _customsProcedureCodes = _sad.SADGood.First().Procedure.RequestedProcedure();
            string _procedureCodeFormat = "{0:D2}XX";
            switch ( _customsProcedureCodes )
            {
              case CustomsProcedureCodes.FreeCirculation:
                _at = "FimdClearence";
                _ret = Clearence.FimdClearence( _edc, _sad.ReferenceNumber );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  _sad.ReleaseForFreeCirculation( _edc, out _comments );
                else
                  _comments = "Document added";
                foreach ( Clearence _clrnx in _ret )
                  _clrnx.CreateTitle( _messageType.ToString() );
                break;
              case CustomsProcedureCodes.InwardProcessing:
                _at = "NewClearence";
                Clearence _newClearance = new Clearence()
                                                          {
                                                            DocumentNo = _sad.DocumentNumber,
                                                            ReferenceNumber = _sad.ReferenceNumber,
                                                            SADConsignmentLibraryIndex = null,
                                                            ProcedureCode = String.Format( _procedureCodeFormat, (int)_customsProcedureCodes ),
                                                            Status = false,
                                                            ClearenceProcedure = ClearenceProcedure._5171,
                                                          };
                _ret = new Clearence[] { _newClearance }.AsQueryable();
                _at = "InsertOnSubmit";
                _edc.Clearence.InsertOnSubmit( _newClearance );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  CreateIPRAccount( _edc, _sad, _newClearance, CustomsDocument.DocumentType.PZC, _sad.CustomsDebtDate.Value, out _comments, customsDocumentLibrary );
                else
                  _comments = "Document added";
                _newClearance.CreateTitle( _messageType.ToString() );
                break;
              case CustomsProcedureCodes.CustomsWarehousingProcedure:
                _at = "NewClearence";
                Clearence _newWarehousinClearance = new Clearence()
                {
                  DocumentNo = _sad.DocumentNumber,
                  ReferenceNumber = _sad.ReferenceNumber,
                  SADConsignmentLibraryIndex = null,
                  ProcedureCode = String.Format( _procedureCodeFormat, (int)_customsProcedureCodes ),
                  Status = false,
                  //[pr4-3738] CustomsProcedureCodes.CustomsWarehousingProcedure 7100 must be added http://itrserver/Bugs/BugDetail.aspx?bid=3738
                  ClearenceProcedure = ClearenceProcedure._7100,
                };
                _ret = new Clearence[] { _newWarehousinClearance }.AsQueryable();
                _newWarehousinClearance.CreateTitle( _messageType.ToString() );
                _at = "InsertOnSubmit";
                _edc.Clearence.InsertOnSubmit( _newWarehousinClearance );
                if ( _messageType == CustomsDocument.DocumentType.PZC )
                  ;// TODO CreateStockRecord  
                else
                  _comments = "Document added";
                break;
              case CustomsProcedureCodes.NoProcedure:
              case CustomsProcedureCodes.ReExport:
              default:
                throw new IPRDataConsistencyException( "Clearence.Associate", string.Format( "Unexpected procedure code for the {0} message", _messageType ), null, _wrongProcedure );
            } //switch (_customsProcedureCodes)
            break;
          case CustomsDocument.DocumentType.IE529:
            _at = "ReExportOfGoods";
            _comments = "Reexport of goods failed";
            _sad.ReExportOfGoods( _edc, _messageType );
            _comments = "Reexport of goods";
            break;
          case CustomsDocument.DocumentType.CLNE:
            _at = "FimdClearence";
            _ret = Clearence.FimdClearence( _edc, _sad.ReferenceNumber );
            foreach ( Clearence _cx in _ret )
            {
              _cx.DocumentNo = _sad.DocumentNumber;
              _cx.CreateTitle( _messageType.ToString() );
              _at = "StartingDocument";
              SADDocumentType _startingDocument = _cx.SADDocumentID;
              _at = "switch RequestedProcedure";
              switch ( _cx.ProcedureCode.RequestedProcedure() )
              {
                case CustomsProcedureCodes.FreeCirculation:
                  _startingDocument.ReleaseForFreeCirculation( _edc, out _comments );
                  break;
                case CustomsProcedureCodes.InwardProcessing:
                  CreateIPRAccount( _edc, _startingDocument, _cx, CustomsDocument.DocumentType.SAD, _sad.CustomsDebtDate.Value, out _comments, customsDocumentLibrary );
                  break;
                case CustomsProcedureCodes.ReExport:
                case CustomsProcedureCodes.NoProcedure:
                case CustomsProcedureCodes.CustomsWarehousingProcedure:
                default:
                  throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected procedure code for CLNE message", null, _wrongProcedure );
              }
            }
            break;
          default:
            throw new IPRDataConsistencyException( "Clearence.Associate", "Unexpected message type.", null, "Unexpected message type." );
        }//switch (_documentType
      }
      catch ( IPRDataConsistencyException _iorex )
      {
        throw _iorex;
      }
      catch ( GenericStateMachineEngine.ActionResult _ar )
      {
        throw _ar;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "Clearence analyses error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, _src );
      }
      return _ret;
    }
    #endregion

    #region private
    private static void CreateIPRAccount
      ( Entities _edc, SADDocumentType _document, Clearence _nc, CustomsDocument.DocumentType _messageType, DateTime _customsDebtDate, out string _comments, SADDocumentLib iprLibraryLookup )
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
        CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR _ipr = new CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR()
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
          InvoiceNo = _iprdata.Invoice,
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
          this.Invoice = (
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
      internal string Invoice { get; private set; }
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
    /// <summary>
    /// Res the export of goods.
    /// </summary>
    /// <param name="sadDocument">The _this.</param>
    /// <param name="_edc">The _edc.</param>
    /// <param name="_messageType">Type of the _message.</param>
    /// <exception cref="GenericStateMachineEngine.ActionResult">if operation cannot be complited.</exception>
    private static void ReExportOfGoods( this SADDocumentType sadDocument, Entities _edc, xml.Customs.CustomsDocument.DocumentType _messageType )
    {
      List<Clearence> _clearanceList = FimdClearence( _edc, sadDocument );
      foreach ( Clearence _clearance in _clearanceList )
      {
        _clearance.SADDocumentID = sadDocument;
        foreach ( var _disposal in _clearance.Disposal )
          //TODO not sure about this.CustomsDebtDate.Value, but it is the ony one date. 
          _disposal.Export( _edc, sadDocument.DocumentNumber, _clearance, sadDocument.CustomsDebtDate.Value );
        _clearance.DocumentNo = sadDocument.DocumentNumber;
        _clearance.ReferenceNumber = sadDocument.ReferenceNumber;
        _clearance.Status = true;
        _clearance.CreateTitle( _messageType.ToString() );
      }
    }
    private static List<Clearence> FimdClearence( Entities _edc, SADDocumentType _this )
    {
      List<Clearence> _clearance = null;
      foreach ( SADGood _sg in _this.SADGood )
      {
        if ( _sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport )
          throw new IPRDataConsistencyException( "Clearence.Create", String.Format( "IE529 contains invalid customs procedure {0}", _sg.Title ), null, "Wrong customs procedure." );
        foreach ( SADRequiredDocuments _rdx in _sg.SADRequiredDocuments )
        {
          if ( _rdx.Code != XMLResources.RequiredDocumentFinishedGoodExportConsignmentCode )
          {
            int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( _rdx.Number );
            if ( _cleranceInt.HasValue )
            {
              _clearance.Add( Element.GetAtIndex<Clearence>( _edc.Clearence, _cleranceInt.Value ) );
              break;
            }
          }
        } // foreach ( SADRequiredDocuments _rdx in _sg.SADRequiredDocuments )
      }//foreach ( SADGood _sg in SADGood )
      if ( _clearance.Count > 0 )
        return _clearance;
      string _template = "Cannot find required document code ={0} for customs document = {1}/ref={2}";
      throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _template, _this.DocumentNumber, _this.ReferenceNumber ) );
    } //private Clearence FimdClearence(
    private const string _wrongProcedure = "Wrong customs procedure";
    #endregion

  }
}
