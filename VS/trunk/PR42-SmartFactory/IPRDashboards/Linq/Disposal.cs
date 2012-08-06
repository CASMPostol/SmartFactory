using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {
    internal void Export( EntitiesDataContext edc, ref double _quantity, CigaretteExportForm _batchAnalysis, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      ClearingType _clearingType = Linq.IPR.ClearingType.PartialWindingUp;
      if ( !closingBatch && _quantity < this.SettledQuantity )
      {
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
          CompensationGood = this.CompensationGood,
          JSOXCustomsSummaryListLookup = null,
          MaterialLookup = this.MaterialLookup,
          No = int.MaxValue,
          SADDate = CAS.SharePoint.Extensions.SPMinimum,
          SADDocumentNo = "N/A",
          SettledQuantity = this.SettledQuantity - _quantity, 
        };
        _newDisposal.SetUpTitle();
        this.CalcualteDutyAndVat( Linq.IPR.ClearingType.PartialWindingUp );
        this.SettledQuantity = _quantity;
        _quantity = 0;
        edc.Disposal.InsertOnSubmit( _newDisposal );
        edc.SubmitChanges();
      }
      else
      {
        _clearingType = this.IPRID.GetClearingType();
        _quantity -= this.SettledQuantity.Value;
      }
      this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
      IPRIngredient _ingredient = new IPRIngredient( this );
      _batchAnalysis.Add( _ingredient );
      edc.SubmitChanges();
    }
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      this.ClearenceListLookup = clearence;
      this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.CalcualteDutyAndVat( ClearingType.Value );
    }
  }
}
