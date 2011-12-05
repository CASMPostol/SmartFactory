using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockXml = CAS.SmartFactory.xml.erp.Stock;
using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Stock
  {
    internal Stock(Dokument entry, EntitiesDataContext edc)
      : this()
    {
      this.StockLibraryLookup = entry;
      this.BalanceLibraryLookup = edc.StockLibrary.GetTopMostDocumentLookup();
      this.Tytuł = ""; //TODO What to assign to it http://itrserver/Bugs/BugDetail.aspx?bid=2910
    }

    internal static void IportXml
      (StockXml document, EntitiesDataContext edc, Dokument entry, ProgressChangedEventHandler progressChanged)
    {
      Stock newStock = new Stock(entry, edc);
      edc.Stock.InsertOnSubmit(newStock);
      List<StockEntry> stockEntities = new List<StockEntry>();
      foreach (StockXmlRow item in document.Row)
      {
        StockEntry nse = new StockEntry(item, newStock);
        nse.ProcessEntry(edc);
        progressChanged(item, new ProgressChangedEventArgs(1, item.Material));
        stockEntities.Add(nse);
      }
      if (stockEntities.Count > 0)
        edc.StockEntry.InsertAllOnSubmit(stockEntities);
    }
  }
}
