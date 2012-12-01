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
    public void AddDisposal( Entities edc, decimal quantity )
    {
      AddDisposal( edc, DisposalEnum.Cartons, ref quantity );
      if ( quantity > 0 )
      {
        string _msg = String.Format( "Cannot add Disposal to IPR {0} because there is not material on tje IPR.", this.Identyfikator.Value );
        throw CAS.SharePoint.Web.GenericStateMachineEngine.ActionResult.Exception
          ( new CAS.SharePoint.ApplicationError( "CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal", "_qunt > 0", _msg, null ), _msg );
      }
    }
    /// <summary>
    /// Insert on submit the disposal.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="clearence">The clearence.</param>
    /// <exception cref="CAS">CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal;_qunt > 0;null</exception>
    public void AddDisposal( Entities edc, decimal quantity, Clearence clearence )
    {
      Disposal _dsp = AddDisposal( edc, DisposalEnum.Tobacco, ref quantity );
      _dsp.Clearance = clearence;
      if ( quantity > 0 )
      {
        string _msg = String.Format( "Cannot add Disposal to IPR  {0} because because the there is not material on tje IPR.", this.Identyfikator.Value );
        throw CAS.SharePoint.Web.GenericStateMachineEngine.ActionResult.Exception
          ( new CAS.SharePoint.ApplicationError( "CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal", "_qunt > 0", _msg, null ), _msg );
      }
    }
    internal void AddDisposal( Entities edc, DisposalEnum _kind, ref decimal _toDispose, Material material, InvoiceContent invoiceContent )
    {
      Disposal _dsp = AddDisposal( edc, _kind, ref _toDispose );
      _dsp.Material = material;
      _dsp.InvoicEContent = invoiceContent;
      SADGood _sg = invoiceContent.InvoiceIndex.ClearenceIndex.Clearence2SadGoodID;
      if ( _sg != null )
        _dsp.FinishClearingThroughCustoms( edc, _sg );
    }
    /// <summary>
    /// Check if record exists.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/></param>
    /// <param name="documentNo">The document number.</param>
    /// <returns></returns>
    public static bool RecordExist( Entities edc, string documentNo )
    {
      return ( from IPR _iprx in edc.IPR where _iprx.DocumentNo.Contains( documentNo ) select _iprx ).Any();
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
    #endregion

    #region ubternal
    internal static bool IsAvailable( Entities edc, string batch, double requestedTobacco )
    {
      return FindIPRAccountsWithNotAllocatedTobacco( edc, batch ).Sum<IPR>( a => a.TobaccoNotAllocated.Value ) >= requestedTobacco;
    }
    internal decimal TobaccoNotAllocatedDec { get { return Convert.ToDecimal( this.TobaccoNotAllocated.Value ); } set { this.TobaccoNotAllocated = Convert.ToDouble( value ); } }
    /// <summary>
    /// Withdraws the specified quantity.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <param name="max">The maximum quantity to be reverted to the record if <paramref name="quantity"/> is less then 0.</param>
    /// <returns></returns>
    internal decimal Withdraw( ref decimal quantity, decimal max )
    {
      decimal _toDispose = 0;
      if ( quantity > 0 )
        _toDispose = Math.Min( quantity, this.TobaccoNotAllocatedDec );
      else
        _toDispose = Math.Max( max, Convert.ToDecimal( this.NetMass.Value ) - this.TobaccoNotAllocatedDec );
      this.TobaccoNotAllocatedDec -= _toDispose;
      quantity -= _toDispose;
      return _toDispose;
    }
    #endregion
    #region private
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    internal Disposal AddDisposal( Entities edc, DisposalEnum status, ref decimal quantity )
    {
      Linq.DisposalStatus _typeOfDisposal = Entities.GetDisposalStatus( status );
      decimal _toDispose = Withdraw( ref quantity, 0 );
      Disposal _newDisposal = new Disposal( this, _typeOfDisposal, _toDispose );
      edc.Disposal.InsertOnSubmit( _newDisposal );
      return _newDisposal;
    }
    #endregion

  }
}
