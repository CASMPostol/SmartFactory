using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR;
using StockXml = CAS.SmartFactory.xml.erp.Stock;
using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;

namespace CAS.SmartFactory.Linq.IPR
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
      bool errors = false;
      foreach (StockXmlRow item in document.Row)
      {
        try
        {
          StockEntry nse = new StockEntry(item, newStock);
          nse.ProcessEntry(edc);
          progressChanged(item, new ProgressChangedEventArgs(1, item.Material));
          stockEntities.Add(nse);
        }
        catch (Exception ex)
        {
          Anons.WriteEntry(edc, "Stock entry import error", ex.Message);
          errors = true;
        }
      }
      if (errors)
        throw new IPRDataConsistencyException(m_Source, m_AbortMessage, null, "Stock import error");
      if (stockEntities.Count > 0)
        edc.StockEntry.InsertAllOnSubmit(stockEntities);
    }
    private const string m_Source = "Stock processing";
    private const string m_AbortMessage = "There are errors while importing the stock message";
  }
}
