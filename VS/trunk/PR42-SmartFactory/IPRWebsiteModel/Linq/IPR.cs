using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class IPR
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="IPR" /> class.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="iprdata">The _iprdata.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="declaration">The declaration.</param>
    public IPR( Entities entities, Account.IPRAccountData iprdata, Clearence clearence, SADDocumentType declaration )
      : this()
    {
      AccountClosed = false;
      AccountBalance = iprdata.NetMass;
      Batch = iprdata.Batch;
      Cartons = iprdata.CartonsMass;
      ClearenceIndex = clearence;
      ClosingDate = CAS.SharePoint.Extensions.SPMinimum;
      ConsentPeriod = iprdata.ConsentLookup.ConsentPeriod;
      Currency = declaration.Currency;
      CustomsDebtDate = iprdata.CustomsDebtDate;
      DocumentNo = clearence.DocumentNo;
      Duty = iprdata.Duty;
      DutyName = iprdata.DutyName;
      Grade = iprdata.GradeName;
      GrossMass = iprdata.GrossMass;
      InvoiceNo = iprdata.Invoice;
      IPRDutyPerUnit = iprdata.DutyPerUnit;
      IPRLibraryIndex = null;
      IPR2ConsentTitle = iprdata.ConsentLookup;
      IPR2PCNPCN = iprdata.PCNTariffCode;
      IPRUnitPrice = iprdata.UnitPrice;
      IPRVATPerUnit = iprdata.VATPerUnit;
      this.JSOXIndex = null;
      this.JSOXSummary = null;
      NetMass = iprdata.NetMass;
      OGLValidTo = iprdata.ValidToDate;
      ProductivityRateMax = iprdata.ConsentLookup.ProductivityRateMax;
      ProductivityRateMin = iprdata.ConsentLookup.ProductivityRateMin;
      SKU = iprdata.SKU;
      TobaccoName = iprdata.TobaccoName;
      TobaccoNotAllocated = iprdata.NetMass;
      Title = "-- creating -- ";
      Value = iprdata.Value;
      VATName = iprdata.VATName;
      VAT = iprdata.VAT;
      ValidFromDate = iprdata.ConsentLookup.ValidFromDate;
      ValidToDate = iprdata.ConsentLookup.ValidToDate;
      if ( iprdata.CartonsMass > 0 )
        AddDisposal( entities, Convert.ToDecimal( iprdata.CartonsMass ) );
    }
    #endregion

    #region public
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      Title = String.Format( "IPR-{0:D4}{1:D6}", DateTime.Today.Year, Identyfikator.Value );
    }
    /// <summary>
    /// Reverts the withdraw.
    /// </summary>
    /// <param name="quantity">The quantity.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void RevertWithdraw( double quantity )
    {
      this.TobaccoNotAllocated += quantity;
    }
    /// <summary>
    /// Adds the disposal.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="quantity">The quantity.</param>
    /// <exception cref="CAS">CAS.SmartFactory.IPR.WebsiteModel.Linq.AddDisposal;_qunt > 0;null</exception>
    public void AddDisposal( Entities edc, decimal quantity )
    {
      AddDisposal( edc, DisposalEnum.Cartons, ref quantity );
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
    /// <summary>
    /// Gets the VAT as decimal value.
    /// </summary>
    /// <value>
    /// The VAT.
    /// </value>
    public decimal VATDec { get { return Convert.ToDecimal( this.VAT.Value ); } }
    /// <summary>
    /// Gets the duty as decimal value.
    /// </summary>
    /// <value>
    /// The duty.
    /// </value>
    public decimal DutyDec { get { return Convert.ToDecimal( this.Duty.Value ); } }

    #region static
    public static IQueryable<IPR> GetAllNew4JSOX( Entities edc )
    {
      return from _iprx in edc.IPR where _iprx.JSOXIndex == null select _iprx;
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

    #endregion

    #region internal
    internal void AddDisposal( Entities edc, DisposalEnum _kind, ref decimal _toDispose, Material material, InvoiceContent invoiceContent )
    {
      Disposal _dsp = AddDisposal( edc, _kind, ref _toDispose );
      _dsp.Material = material;
      _dsp.InvoicEContent = invoiceContent;
      SADGood _sg = invoiceContent.InvoiceIndex.ClearenceIndex.Clearence2SadGoodID;
      if ( _sg != null )
        _dsp.FinishClearingThroughCustoms( edc, _sg );
    }
    internal void AddDisposal( Entities edc, DisposalEnum _kind, ref decimal _toDispose, Material material )
    {
      Disposal _dsp = AddDisposal( edc, _kind, ref _toDispose );
      _dsp.Material = material;
    }
    internal static decimal IsAvailable( Entities edc, string batch, decimal requestedTobacco )
    {
      return FindIPRAccountsWithNotAllocatedTobacco( edc, batch ).Sum<IPR>( a => a.TobaccoNotAllocatedDec ) - requestedTobacco;
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
      if ( quantity >= 0 )
        _toDispose = Math.Min( quantity, this.TobaccoNotAllocatedDec );
      else
        _toDispose = Math.Max( quantity, -max );
      this.TobaccoNotAllocatedDec -= _toDispose;
      quantity -= _toDispose;
      return _toDispose;
    }
    internal void RecalculateClearedRecords( double startIndex )
    {
      if ( this.AccountClosed.Value )
        throw new ApplicationException( "IPR.RecalculateClearedRecords cannot be excuted for closed account" );
      List<Disposal> _2Calculate = ( from _dx in this.Disposal where _dx.CustomsStatus.Value == Linq.CustomsStatus.Finished orderby _dx.No.Value ascending select _dx ).ToList<Disposal>();
      this.AccountBalance = this.NetMass;
      foreach ( Disposal _dx in _2Calculate )
        _dx.CalculateRemainingQuantity();
    }
    #endregion

    #region private
    /// <summary>
    /// Contains calculated data required to create IPR account
    /// </summary>
    private Disposal AddDisposal( Entities edc, DisposalEnum status, ref decimal quantity )
    {
      if ( quantity <= 0 && status != DisposalEnum.Cartons )
        throw new ArgumentException( "IPR.AddDisposal(Entities, DisposalEnum, ref decimal): internal consistency check quantity <= 0", "quantity" );
      Linq.DisposalStatus _typeOfDisposal = Entities.GetDisposalStatus( status );
      decimal _toDispose = 0;
      if ( status == DisposalEnum.Cartons )
      {
        _toDispose = quantity;
        quantity = 0;
      }
      else
        _toDispose = Withdraw( ref quantity, 0 );
      Disposal _newDisposal = new Disposal( this, _typeOfDisposal, _toDispose );
      edc.Disposal.InsertOnSubmit( _newDisposal );
      return _newDisposal;
    }
    #endregion

  }
}
