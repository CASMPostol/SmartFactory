using System;
using System.Collections.Generic;
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
      PreviousMonthDate = previous.SituationDate.GetValueOrDefault( DateTime.Today.Date - TimeSpan.FromDays( 30 ) );
      PreviousMonthQuantity = previous.SituationQuantity.GetValueOrDefault( -1 );
      UpdateJSOXReport( edc );

    }
    //internal Dictionary<string, BalanceTotals> CalculateBalanceBatch( Entities edc )
    //{
    //  IQueryable<IGrouping<string, IPR>> _accountGroups = from _iprx in IPR.GetAllOpen4JSOX( edc ) group _iprx by _iprx.Batch;
    //  Dictionary<string, BalanceTotals> _ret = new Dictionary<string, BalanceTotals>();
    //  foreach ( IGrouping<string, IPR> _btchx in _accountGroups )
    //    _ret.Add( _btchx.Key, new BalanceTotals( _btchx ) );
    //  return _ret;
    //}

    /// <summary>
    /// Updates the JSOX report.
    /// </summary>
    /// <param name="edc">The edc.</param>
    public void UpdateJSOXReport( Entities edc )
    {
      Dictionary<string, IGrouping<string, IPR>> _accountGroups = ( from _iprx in Linq.IPR.GetAllOpen4JSOX( edc ) group _iprx by _iprx.Batch ).ToDictionary( x => x.Key );
      List<string> _processed = new List<string>();
      foreach ( BalanceBatch _bbx in BalanceBatch )
      {
        if ( _accountGroups.ContainsKey( _bbx.Batch ) )
          _bbx.Update( _accountGroups[ _bbx.Batch ] );
        else
          edc.BalanceBatch.DeleteOnSubmit( _bbx );
        _processed.Add( _bbx.Batch );
      }
      foreach ( string _btchx in _processed )
        _accountGroups.Remove( _btchx );
      foreach ( var _grpx in _accountGroups )
        Linq.BalanceBatch.Create( edc, _grpx.Value, this );

      //Introducing
      DateTime _thisIntroducingDateStart = LinqIPRExtensions.DateTimeMaxValue;
      DateTime _thisIntroducingDateEnd = LinqIPRExtensions.DateTimeMinValue;
      decimal _introducingQuantity = Linq.IPR.GetIntroducingData( edc, this, out _thisIntroducingDateStart, out _thisIntroducingDateEnd );
      this.IntroducingDateStart = _thisIntroducingDateStart;
      this.IntroducingDateEnd = _thisIntroducingDateEnd;
      this.IntroducingQuantity = _introducingQuantity.Convert2Double2Decimals();

      //Outbound
      DateTime _thisOutboundDateEnd = LinqIPRExtensions.DateTimeMinValue;
      DateTime _thisOutboundDateStart = LinqIPRExtensions.DateTimeMaxValue;
      decimal _outQuantity = Linq.JSOXCustomsSummary.GetOutboundQuantity( edc, this, out _thisOutboundDateStart, out _thisOutboundDateEnd );
      this.OutboundQuantity = _outQuantity.Convert2Double2Decimals();
      this.OutboundDateEnd = _thisOutboundDateEnd;
      this.OutboundDateStart = _thisOutboundDateStart;

      //Balance
      decimal _thisBalanceQuantity = Convert.ToDecimal( this.PreviousMonthQuantity ) + _introducingQuantity - _outQuantity;
      this.BalanceDate = DateTime.Today.Date;
      this.BalanceQuantity = _thisBalanceQuantity.Convert2Double2Decimals();

      //Situation at
      DateTime _thisSituationDate;
      decimal _thisSituationQuantity = Linq.IPR.GetCurrentSituationData( edc, out _thisSituationDate );
      this.SituationDate = DateTime.Today.Date;
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
