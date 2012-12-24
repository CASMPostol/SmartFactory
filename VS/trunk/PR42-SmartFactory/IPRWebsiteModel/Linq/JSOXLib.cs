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
    /// JSOX report.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <exception cref="InputDataValidationException">Cannot find previous JSOX report; JSOXReport;The JSOX reports lis is empty;true</exception>
    public void JSOXReport( Entities edc )
    {
      ////Previous
      //JSOXLib _prev = new JSOXLib();
      //JSOXReport( edc, _prev );
      //edc.JSOXLibrary.InsertOnSubmit( _prev );
      //this.PreviousMonthDate = previous.SituationDate.GetValueOrDefault( DateTime.Today.Date - TimeSpan.FromDays( 30 ) );
      //this.PreviousMonthQuantity = previous.SituationQuantity.GetValueOrDefault( 0 );
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
