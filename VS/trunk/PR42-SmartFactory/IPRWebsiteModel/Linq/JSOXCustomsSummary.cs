using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class JSOXCustomsSummary
  {
    internal static void CreateEntries( Entities edc, JSOXLib parent )
    {
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
        _dspx.JSOXCustomsSummaryIndex = _newItem;
        _newItem.CreateTitle();
      }
    }

    private void CreateTitle()
    {
     this.Title = this.SADConsignmentNo;
    }
  }
}
