using System;
using System.Collections.Generic;
using System.Linq;

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
      this.IPR2JSOXIndex = null;
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
    internal decimal NetMassDec { get { return Convert.ToDecimal( this.NetMass.Value ); } }

    #region static
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
      return ( from IPR _iprx in edc.IPR
               where _iprx.Batch.Contains( batch ) && !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0
               orderby _iprx.CustomsDebtDate.Value ascending, _iprx.DocumentNo ascending
               select _iprx ).ToList();
    }
    /// <summary>
    /// Gets the introducing quantity.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="dateStart">The date start.</param>
    /// <param name="dateEnd">The date end.</param>
    /// <returns></returns>
    public static decimal GetIntroducingData( Entities edc, JSOXLib parent, out DateTime dateStart, out DateTime dateEnd )
    {
      decimal _ret = 0;
      dateEnd = LinqIPRExtensions.DateTimeMinValue;
      dateStart = LinqIPRExtensions.DateTimeMaxValue;
      foreach ( IPR _iprx in parent.IPR )
      {
        _ret += _iprx.NetMassDec;
        dateEnd = LinqIPRExtensions.Max( _iprx.CustomsDebtDate.Value.Date, dateEnd );
        dateStart = LinqIPRExtensions.Min( _iprx.CustomsDebtDate.Value.Date, dateStart );
      }
      foreach ( IPR _iprx in IPR.GetAllNew4JSOX( edc ) )
      {
        _iprx.IPR2JSOXIndex = parent;
        _ret += _iprx.NetMassDec;
        dateEnd = LinqIPRExtensions.Max( _iprx.CustomsDebtDate.Value.Date, dateEnd );
        dateStart = LinqIPRExtensions.Min( _iprx.CustomsDebtDate.Value.Date, dateStart );
      }
      return _ret;
    }
    /// <summary>
    /// Gets the current situation.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="dateEnd">The date end.</param>
    /// <returns></returns>
    public static decimal GetCurrentSituationData( Entities edc )
    {
      decimal _ret = 0;
      foreach ( IPR _iprx in IPR.GetAllOpen4JSOX( edc ) )
        _ret += _iprx.AccountBalanceDec;
      return _ret;
    }
    #endregion

    #endregion

    #region internal
    internal enum ValueKey
    {
      DustCSNotStarted,
      DustCSStarted,
      OveruseCSNotStarted,
      OveruseCSStarted,
      PureTobaccoCSNotStarted,
      PureTobaccoCSStarted,
      SHMentholCSNotStarted,
      SHMentholCSStarted,
      TobaccoCSFinished,
      TobaccoInFGCSNotStarted,
      TobaccoInFGCSStarted,
      WasteCSNotStarted,
      WasteCSStarted,

      //calculated
      IPRBook,
      SHWasteOveruseCSNotStarted,
      TobaccoAvailable,
      TobaccoEnteredIntoIPR,
      TobaccoToBeUsedInTheProduction,
      TobaccoUsedInTheProduction
    }
    internal class Balance: Dictionary<IPR.ValueKey, decimal>
    {
      #region ctor
      internal Balance( IPR record )
      {
        foreach ( ValueKey _vkx in Enum.GetValues( typeof( ValueKey ) ) )
          base[ _vkx ] = 0;
        #region totals
        foreach ( Disposal _dspx in record.Disposal )
        {
          switch ( _dspx.CustomsStatus.Value )
          {
            case CustomsStatus.NotStarted:
              switch ( _dspx.DisposalStatus.Value )
              {
                case DisposalStatus.Dust:
                  base[ ValueKey.DustCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.SHMenthol:
                  base[ ValueKey.SHMentholCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Waste:
                  base[ ValueKey.WasteCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Overuse:
                  base[ ValueKey.OveruseCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Tobacco:
                  base[ ValueKey.PureTobaccoCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.TobaccoInCigaretes:
                  base[ ValueKey.TobaccoInFGCSNotStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Cartons:
                case DisposalStatus.TobaccoInCigaretesDestinationEU:
                case DisposalStatus.TobaccoInCigaretesProduction:
                case DisposalStatus.TobaccoInCutfiller:
                case DisposalStatus.TobaccoInCutfillerDestinationEU:
                case DisposalStatus.TobaccoInCutfillerProduction:
                  break;
              }
              break;
            case CustomsStatus.Started:
              switch ( _dspx.DisposalStatus.Value )
              {
                case DisposalStatus.Dust:
                  base[ ValueKey.DustCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.SHMenthol:
                  base[ ValueKey.SHMentholCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Waste:
                  base[ ValueKey.WasteCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Overuse:
                  base[ ValueKey.OveruseCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Tobacco:
                  base[ ValueKey.PureTobaccoCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.TobaccoInCigaretes:
                  base[ ValueKey.TobaccoInFGCSStarted ] += _dspx.SettledQuantityDec;
                  break;
                case DisposalStatus.Cartons:
                case DisposalStatus.TobaccoInCigaretesDestinationEU:
                case DisposalStatus.TobaccoInCigaretesProduction:
                case DisposalStatus.TobaccoInCutfiller:
                case DisposalStatus.TobaccoInCutfillerDestinationEU:
                case DisposalStatus.TobaccoInCutfillerProduction:
                  break;
              }
              break;
            case CustomsStatus.Finished:
              base[ ValueKey.TobaccoCSFinished ] += _dspx.SettledQuantityDec;
              break;
          }
        }
        #endregion
        base[ ValueKey.TobaccoEnteredIntoIPR ] = record.NetMassDec;
        base[ ValueKey.IPRBook ] = IPRBook;
        base[ ValueKey.SHWasteOveruseCSNotStarted ] = SHWasteOveruseCSNotStarted;
        base[ ValueKey.TobaccoAvailable ] = TobaccoAvailable;
        base[ ValueKey.TobaccoUsedInTheProduction ] = TobaccoUsedInTheProduction;
        base[ ValueKey.TobaccoToBeUsedInTheProduction ] = base[ ValueKey.TobaccoEnteredIntoIPR ] - base[ ValueKey.TobaccoUsedInTheProduction ];
      }
      #endregion

      #region internal
      internal new double this[ ValueKey index ]
      {
        get { return Convert.ToDouble( base[ index ] ); }
      }
      internal Dictionary<IPR.ValueKey, decimal> Base { get { return this; } }
      #endregion

      #region private
      private decimal IPRBook
      {
        get
        {
          return
            base[ ValueKey.TobaccoEnteredIntoIPR ] -
            base[ ValueKey.TobaccoCSFinished ] -
            base[ ValueKey.TobaccoInFGCSStarted ] -
            base[ ValueKey.DustCSStarted ] -
            base[ ValueKey.WasteCSStarted ] -
            base[ ValueKey.SHMentholCSStarted ] -
            base[ ValueKey.OveruseCSStarted ] -
            base[ ValueKey.PureTobaccoCSStarted ];
        }
      }
      private decimal SHWasteOveruseCSNotStarted
      {
        get
        {
          return
            base[ ValueKey.WasteCSNotStarted ] +
            base[ ValueKey.SHMentholCSNotStarted ] +
            base[ ValueKey.OveruseCSNotStarted ] +
            base[ ValueKey.PureTobaccoCSNotStarted ];
        }
      }
      public decimal TobaccoAvailable
      {
        get
        {
          return
            base[ ValueKey.IPRBook ] -
            base[ ValueKey.SHWasteOveruseCSNotStarted ] -
            base[ ValueKey.DustCSNotStarted ];
        }
      }
      private decimal TobaccoUsedInTheProduction
      {
        get
        {
          return
            base[ ValueKey.TobaccoInFGCSNotStarted ] +
            base[ ValueKey.DustCSNotStarted ] +
            base[ ValueKey.SHWasteOveruseCSNotStarted ] +
            base[ ValueKey.TobaccoCSFinished ] +
            base[ ValueKey.TobaccoInFGCSStarted ] +
            base[ ValueKey.DustCSStarted ] +
            base[ ValueKey.WasteCSStarted ] +
            base[ ValueKey.SHMentholCSStarted ] +
            base[ ValueKey.OveruseCSStarted ] +
            base[ ValueKey.PureTobaccoCSStarted ];
        }
      }
      #endregion

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
    internal static IQueryable<IGrouping<string, IPR>> GetAllOpen4JSOXGroups( Entities edc )
    {
      return from _iprx in edc.IPR
             where !_iprx.AccountClosed.Value
             orderby _iprx.CustomsDebtDate.Value ascending
             group _iprx by _iprx.Batch;
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
    private static IQueryable<IPR> GetAllNew4JSOX( Entities edc )
    {
      return from _iprx in edc.IPR where _iprx.IPR2JSOXIndex == null select _iprx;
    }
    /// <summary>
    /// Gets all open account.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    private static IQueryable<IPR> GetAllOpen4JSOX( Entities edc )
    {
      return from _iprx in edc.IPR
             where !_iprx.AccountClosed.Value
             orderby _iprx.CustomsDebtDate.Value
             select _iprx;
    }
    private decimal AccountBalanceDec { get { return Convert.ToDecimal( AccountBalance.Value ); } }
    #endregion

  }
}
