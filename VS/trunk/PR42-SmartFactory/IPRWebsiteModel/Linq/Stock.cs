using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Stock
  {
    internal Stock(Dokument entry, Entities edc)
      : this()
    {
      this.StockLibraryIndex = entry;
      this.BalanceLibraryIndex = edc.StockLibrary.GetTopMostDocumentLookup();
      this.Title = ""; //TODO What to assign to it http://itrserver/Bugs/BugDetail.aspx?bid=2910
    }
  }
}
