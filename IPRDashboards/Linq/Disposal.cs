using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {
    internal void Export( EntitiesDataContext edc, ref double _quantity, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      string _at = "startting";
      try
      {
        ClearingType _clearingType = Linq.IPR.ClearingType.PartialWindingUp;
        if ( !closingBatch && _quantity < this.SettledQuantity )
        {
          _at = "_newDisposal";
          Disposal _newDisposal = new Disposal()
          {
            BatchLookup = this.BatchLookup,
            ClearenceListLookup = null,
            ClearingType = Linq.IPR.ClearingType.PartialWindingUp,
            CustomsStatus = Linq.IPR.CustomsStatus.NotStarted,
            CustomsProcedure = "N/A",
            DisposalStatus = this.DisposalStatus,
            InvoiceNo = "N/A",
            IPRDocumentNo = "N/A", // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
            IPRID = this.IPRID,
            // CompensationGood = this.CompensationGood, //TODO [pr4-3585] Wrong value for CompensationGood http://itrserver/Bugs/BugDetail.aspx?bid=3585
            JSOXCustomsSummaryListLookup = null,
            MaterialLookup = this.MaterialLookup,
            No = int.MaxValue,
            SADDate = CAS.SharePoint.Extensions.SPMinimum,
            SADDocumentNo = "N/A",
            SettledQuantity = this.SettledQuantity - _quantity
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( Linq.IPR.ClearingType.PartialWindingUp );
          this.SettledQuantity = _quantity;
          _quantity = 0;
          _at = "InsertOnSubmit";
          edc.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          edc.SubmitChanges();
        }
        else
        {
          _clearingType = this.IPRID.GetClearingType();
          _quantity -= this.SettledQuantity.Value;
        }
        _at = "StartClearance";
        this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
        _at = "new IPRIngredient";
        IPRIngredient _ingredient = IPRIngredientFactory.IPRIngredient( this );
        ingredient.Add( _ingredient );
        _at = "SubmitChanges #2";
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        string _tmpl = "Cannot proceed with export of disposal: {0} because of error: {1}.";
        throw new ApplicationError( "Disposal.Export", _at, String.Format( _tmpl, this.Identyfikator, ex.Message ), ex );
      }
    }
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      this.ClearenceListLookup = clearence;
      this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.SetUpCalculatedColumns( ClearingType.Value );
    }
  }
}
