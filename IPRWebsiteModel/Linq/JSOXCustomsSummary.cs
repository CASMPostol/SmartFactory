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
    internal static decimal CreateEntries( Entities edc, JSOXLib parent )
    {
      decimal _ret = 0;
      List<JSOXCustomsSummary> _newEntries = new List<JSOXCustomsSummary>();
      foreach ( Disposal _dspx in Linq.Disposal.GetEntries4JSOX( edc ) )
      {
        JSOXCustomsSummary _newItem = new JSOXCustomsSummary()
        {
          CompensationGood = _dspx.Disposal2PCNID.CompensationGood,
          ExportOrFreeCirculationSAD = _dspx.SADDocumentNo,
          InvoiceNo = _dspx.InvoiceNo,
          JSOXIndex = parent,
          SADConsignmentDate = _dspx.Disposal2IPRIndex.CustomsDebtDate.ToString(), //TODO Wrong  type of SADConsignmentDate
          SADConsignmentNo = _dspx.Disposal2IPRIndex.DocumentNo,
          SADDate = _dspx.SADDate,
          TotalAmount = _dspx.SettledQuantity,
          Title = "Creating"
        };
        _ret += _dspx.SettledQuantityDec;
        _dspx.JSOXCustomsSummaryIndex = _newItem;
        _newItem.CreateTitle();
        _newEntries.Add( _newItem );
      }
      return _ret;
    }

    private void CreateTitle()
    {
      this.Title = this.SADConsignmentNo;
    }
  }
}
