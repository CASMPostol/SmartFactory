using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class JSOXCustomsSummary
  {
    /// <summary>
    /// Creates the entries.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="parent">The parent library.</param>
    /// <returns>Returns total sum of settled quantity fro all disposals.</returns>
    internal static decimal CreateEntries( Entities edc, JSOXLib jSOXLib, out DateTime? nullable1, out DateTime? nullable2 )
    {
      throw new NotImplementedException();
    }
    internal static decimal CreateEntries( Entities edc, JSOXLib parent, out DateTime start, out DateTime end )
    {
      decimal _ret = 0;
      List<JSOXCustomsSummary> _newEntries = new List<JSOXCustomsSummary>();
      start = LinqIPRExtensions.DateTimeMaxValue;
      end = LinqIPRExtensions.DateTimeMinValue;
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
      foreach ( JSOXCustomsSummary _jcsx in parent.JSOXCustomsSummary )
      {
        start = LinqIPRExtensions.Min( start, _jcsx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMaxValue ) );
        end = LinqIPRExtensions.Max( end, _jcsx.SADDate.GetValueOrDefault( LinqIPRExtensions.DateTimeMinValue ) );
        _ret += _jcsx.SettledQuantityDec;
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
