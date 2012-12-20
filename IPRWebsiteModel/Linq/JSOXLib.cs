using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// JSOXLib
  /// </summary>
  public partial class JSOXLib
  {
    /// <summary>
    /// JSOX report.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <exception cref="InputDataValidationException">Cannot find previous JSOX report;JSOXReport;The JSOX reports lis is empty;true</exception>
    public void JSOXReport( Entities edc )
    {
      JSOXLib _prev = Previous( edc ).FirstOrDefault<JSOXLib>();
      if ( _prev == null )
        throw new InputDataValidationException( "Cannot find previous JSOX report", "JSOXReport", "The JSOX reports lis is empty", true );
      this.PreviousMonthDate = _prev.SituationDate;
      this.PreviousMonthQuantity = _prev.SituationQuantity;
      //Introducing
      decimal _introducingQuantity = 0;
      this.IntroducingDateEnd = DateTime.MinValue;
      this.IntroducingDateStart = DateTime.MaxValue;
      foreach ( IPR _iprx in IPR.GetAllNew4JSOX( edc ) )
      {
        _iprx.JSOXIndex = this;
        _introducingQuantity += _iprx.NetMassDec;
        this.IntroducingDateEnd = LinqIPRExtensions.Max( _iprx.CustomsDebtDate.Value.Date, this.IntroducingDateEnd.Value );
        this.IntroducingDateStart = LinqIPRExtensions.Min( _iprx.CustomsDebtDate.Value.Date, this.IntroducingDateEnd.Value );
      }
      this.IntroducingQuantity = Convert.ToDouble( _introducingQuantity );
      //Outbound
      DateTime _thisOutboundDateEnd = DateTime.MinValue;
      DateTime _thisOutboundDateStart = DateTime.MaxValue;
      decimal _outQuantity = JSOXCustomsSummary.CreateEntries( edc, this, out _thisOutboundDateStart, out _thisOutboundDateEnd );
      this.OutboundQuantity = Convert.ToDouble( _outQuantity );
      this.OutboundDateEnd = _thisOutboundDateEnd;
      this.OutboundDateStart = _thisOutboundDateStart;
       //this.BalanceDate = xzxzx;
      //this.BalanceQuantity = xzxzx;
    }
    public IQueryable<JSOXLib> Previous( Entities edc ) { return from _jsx in edc.JSOXLibrary orderby _jsx.BalanceDate.Value descending select _jsx; }

  }
}
