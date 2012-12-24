using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// JSOXLib
  /// </summary>
  public partial class JSOXLib
  {

    #region public
    /// <summary>
    /// Updates the JSOX report.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="previous">The previous report.</param>
    public void UpdateJSOXReport( Entities edc, JSOXLib previous )
    {
      PreviousMonthDate = this.SituationDate.GetValueOrDefault( DateTime.Today.Date - TimeSpan.FromDays( 30 ) );
      PreviousMonthQuantity = this.SituationQuantity.GetValueOrDefault( 0 );
      UpdateJSOXReport( edc );
    }
    /// <summary>
    /// Updates the JSOX report.
    /// </summary>
    /// <param name="edc">The edc.</param>
    public void UpdateJSOXReport( Entities edc )
    {

      //Introducing
      DateTime _thisIntroducingDateStart = LinqIPRExtensions.DateTimeMaxValue;
      DateTime _thisIntroducingDateEnd = LinqIPRExtensions.DateTimeMinValue;
      //TODO IPR reverse lookup required - summ already added IPR account
      decimal _introducingQuantity = IPR.GetIntroducingData( edc, this, out _thisIntroducingDateStart, out _thisIntroducingDateEnd );
      this.IntroducingDateStart = _thisIntroducingDateStart;
      this.IntroducingDateEnd = _thisIntroducingDateEnd;
      this.IntroducingQuantity = _introducingQuantity.Convert2Double2Decimals();

      //Outbound
      DateTime _thisOutboundDateEnd = LinqIPRExtensions.DateTimeMinValue;
      DateTime _thisOutboundDateStart = LinqIPRExtensions.DateTimeMaxValue;
      //TODO JSOXCustomsSummary reverse lookup - summ already added disposals account
      decimal _outQuantity = JSOXCustomsSummary.CreateEntries( edc, this, out _thisOutboundDateStart, out _thisOutboundDateEnd );
      this.OutboundQuantity = _outQuantity.Convert2Double2Decimals();
      this.OutboundDateEnd = _thisOutboundDateEnd;
      this.OutboundDateStart = _thisOutboundDateStart;

      //Balance
      decimal _thisBalanceQuantity = Convert.ToDecimal( this.PreviousMonthQuantity ) + _introducingQuantity - _outQuantity;
      this.BalanceDate = DateTime.Today.Date;
      this.BalanceQuantity = _thisBalanceQuantity.Convert2Double2Decimals();

      //Situation at
      DateTime _thisSituationDate;
      decimal _thisSituationQuantity = IPR.GetCurrentSituationData( edc, out _thisSituationDate );
      this.SituationDate = _thisSituationDate;
      this.SituationQuantity = _thisSituationQuantity.Convert2Double2Decimals();

      //Reassume
      this.ReassumeQuantity = ( _thisBalanceQuantity - _thisSituationQuantity ).Convert2Double2Decimals();

    }
    #endregion

    #region private
    private IQueryable<JSOXLib> Previous( Entities edc ) { return from _jsx in edc.JSOXLibrary orderby _jsx.BalanceDate.Value descending select _jsx; }
    #endregion

  }
}
