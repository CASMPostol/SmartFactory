using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class IPR
  {
    public ClearingType GetClearingType()
    {
      return this.TobaccoNotAllocated == 0 &&
        (
          from _dec in this.Disposal
          where _dec.CustomsStatus.Value == CustomsStatus.NotStarted
          select _dec
        ).Count() == 1 ? ClearingType.TotalWindingUp : ClearingType.PartialWindingUp;
    }
    public void Withdraw( double p )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
    public void RevertWithdraw( double? nullable )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
    public enum DisposalEnum { Dust, SHMenthol, Waste, OverusageInKg, Tobacco };
    public static List<IPR> FindIPRAccountsWithNotAllocatedTobacco( Entities _edc, string _batch )
    {
      return ( from IPR _iprx in _edc.IPR where _iprx.Batch.Contains( _batch ) && !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0 orderby _iprx.Identyfikator ascending select _iprx ).ToList();
    }
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    public void AddDisposal( Entities _edc, DisposalEnum _status, ref double quantity, Material material )
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
  }
}
