using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class JSOXCustomsSummary
  {
    internal static decimal GetOutboundQuantity( Entities edc, JSOXLib parent, out DateTime start, out DateTime end )
    {
      decimal _ret = 0;
      List<JSOXCustomsSummary> _newEntries = new List<JSOXCustomsSummary>();
      start = LinqIPRExtensions.DateTimeMaxValue;
      end = LinqIPRExtensions.DateTimeMinValue;
      foreach ( JSOXCustomsSummary _jcsx in parent.JSOXCustomsSummary )
      {
        start = LinqIPRExtensions.Min( start, _jcsx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMaxValue ) );
        end = LinqIPRExtensions.Max( end, _jcsx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMinValue ) );
        _ret += _jcsx.SettledQuantityDec;
      }
      foreach ( Disposal _dspx in Linq.Disposal.GetEntries4JSOX( edc ) )
      {
        start = LinqIPRExtensions.Min( start, _dspx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMaxValue ) );
        end = LinqIPRExtensions.Max( end, _dspx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMinValue ) );
        JSOXCustomsSummary _newItem = new JSOXCustomsSummary()
        {
          CompensationGood = _dspx.Disposal2PCNID == null ? "TBD" : _dspx.Disposal2PCNID.CompensationGood,
          ExportOrFreeCirculationSAD = _dspx.SADDocumentNo,
          InvoiceNo = _dspx.InvoiceNo,
          JSOXCustomsSummary2JSOXIndex = parent,
          IntroducingSADDate = _dspx.Disposal2IPRIndex.CustomsDebtDate.Value.Date,
          IntroducingSADNo = _dspx.Disposal2IPRIndex.DocumentNo,
          SADDate = _dspx.SADDate,
          TotalAmount = _dspx.SettledQuantity,
          Title = "Creating"
        };
        _ret += _dspx.SettledQuantityDec;
        _dspx.JSOXCustomsSummaryIndex = _newItem;
        _newItem.CreateTitle();
        _newEntries.Add( _newItem );
      }
      edc.JSOXCustomsSummary.InsertAllOnSubmit( _newEntries );
      return _ret;
    }

    private void CreateTitle()
    {
      this.Title = this.IntroducingSADNo;
    }
    private decimal SettledQuantityDec { get { return Convert.ToDecimal( this.TotalAmount.Value ); } }

  }
}
