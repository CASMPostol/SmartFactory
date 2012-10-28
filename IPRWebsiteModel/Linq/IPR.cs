﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class IPR
  {
    #region public
    /// <summary>
    /// Enumerated kinds of Disposal 
    /// </summary>
    public enum DisposalEnum { Dust, SHMenthol, Waste, OverusageInKg, Tobacco };
    /// <summary>
    /// Gets the type of the clearing.
    /// </summary>
    /// <returns></returns>
    public ClearingType GetClearingType()
    {
      return this.TobaccoNotAllocated == 0 &&
        (
          from _dec in this.Disposal
          where _dec.CustomsStatus.Value == CustomsStatus.NotStarted
          select _dec
        ).Count() == 1 ? ClearingType.TotalWindingUp : ClearingType.PartialWindingUp;
    }
    /// <summary>
    /// Reverts the withdraw.
    /// </summary>
    /// <param name="nullable">The nullable.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void RevertWithdraw( double quantity )
    {
      this.TobaccoNotAllocated += quantity;
    }
    /// <summary>
    /// Finds the IPR accounts with not allocated tobacco.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="batch">The _batch.</param>
    /// <returns></returns>
    public static List<IPR> FindIPRAccountsWithNotAllocatedTobacco( Entities edc, string batch )
    {
      return ( from IPR _iprx in edc.IPR where _iprx.Batch.Contains( batch ) && !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0 orderby _iprx.Identyfikator ascending select _iprx ).ToList();
    }
    /// <summary>
    /// Adds the disposal.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="clearence">The clearence.</param>
    /// <exception cref="CAS">CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal;_qunt > 0;null</exception>
    public void AddDisposal( Entities edc, decimal quantity, Clearence clearence )
    {
      decimal _qunt = quantity;
      AddDisposal( edc, DisposalEnum.Tobacco, ref quantity, null, clearence );
      if ( _qunt > 0 )
      {
        string _msg = String.Format( "Cannot add Disposal to IPR  {0} because because the there is not material on tje IPR.", this.Identyfikator.Value );
        throw CAS.SharePoint.Web.GenericStateMachineEngine.ActionResult.Exception
          ( new CAS.SharePoint.ApplicationError( "CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal", "_qunt > 0", _msg, null ), _msg );
      }
    }
    /// <summary>
    /// Adds the disposal.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="status">The _status.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="material">The material.</param>
    public void AddDisposal( Entities edc, DisposalEnum status, ref decimal quantity, Material material )
    {
      AddDisposal( edc, status, ref quantity, material, null );
    }
    #endregion

    #region private
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    private void AddDisposal( Entities _edc, DisposalEnum _status, ref decimal quantity, Material material, Clearence clearence )
    {
      try
      {
        Linq.DisposalStatus _typeOfDisposal = default( Linq.DisposalStatus );
        PCNCode _disposal2PCNID = null; // PCNCompensationGood should be secoundary lookup. http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3333
        string _pcncompensationGood = String.Empty.NotAvailable();
        switch ( _status )
        {
          case DisposalEnum.Dust:
            _typeOfDisposal = Linq.DisposalStatus.Dust;
            break;
          case DisposalEnum.SHMenthol:
            _typeOfDisposal = Linq.DisposalStatus.SHMenthol;
            break;
          case DisposalEnum.Waste:
            _typeOfDisposal = Linq.DisposalStatus.Waste;
            break;
          case DisposalEnum.OverusageInKg:
            _typeOfDisposal = Linq.DisposalStatus.Overuse;
            break;
          case DisposalEnum.Tobacco:
            _typeOfDisposal = Linq.DisposalStatus.Tobacco;
            _disposal2PCNID = IPR2PCNPCN;
            _pcncompensationGood = IPR2PCNPCN.Title();
            break;
        }
        double _toDispose = Withdraw( ref quantity );
        Disposal _newDisposal = new Disposal()
        {
          ClearingType = Linq.ClearingType.PartialWindingUp,
          CustomsStatus = Linq.CustomsStatus.NotStarted,
          CustomsProcedure = clearence == null ? string.Empty.NotAvailable() : Entities.ToString( clearence.ClearenceProcedure.Value ),
          Disposal2BatchIndex = material == null ? null : material.Material2BatchIndex,
          Disposal2ClearenceIndex = clearence,
          Disposal2IPRIndex = this,
          Disposal2MaterialIndex = material,
          DisposalStatus = _typeOfDisposal,
          Disposal2PCNID = _disposal2PCNID,
          PCNCompensationGood = String.Empty.NotAvailable(),
          DutyAndVAT = new Nullable<double>(),  // calculated in SetUpCalculatedColumns,
          DutyPerSettledAmount = new Nullable<double>(),  // calculated in SetUpCalculatedColumns,
          InvoiceNo = String.Empty.NotAvailable(), //To be assigned during finished goods export.
          IPRDocumentNo = String.Empty.NotAvailable(), // to be assigned while sad processing
          JSOXCustomsSummaryIndex = null,
          No = new Nullable<double>(),
          RemainingQuantity = new Nullable<double>(), //To be set during sad processing
          SADDate = CAS.SharePoint.Extensions.SPMinimum,
          SADDocumentNo = String.Empty.NotAvailable(),
          SettledQuantity = _toDispose,
          Title = String.Empty, // calculated in SetUpCalculatedColumns,
          VATPerSettledAmount = new Nullable<double>(), //calculated in SetUpCalculatedColumns,
          TobaccoValue = new Nullable<double>() //calculated in SetUpCalculatedColumns,
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
    /// <summary>
    /// Withdraws the specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <returns></returns>
    private double Withdraw( ref decimal quantity )
    {
      double _toDispose;
      _toDispose = Math.Min( Convert.ToDouble( quantity ), this.TobaccoNotAllocated.Value );
      this.TobaccoNotAllocated -= _toDispose;
      quantity -= Convert.ToDecimal( _toDispose );
      return _toDispose;
    }
    #endregion

  }
}
