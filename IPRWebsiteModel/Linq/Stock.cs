using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Stock
  {
    public Stock( Dokument entry, Entities edc )
      : this()
    {
      this.StockLibraryIndex = entry;
      this.BalanceLibraryIndex = edc.StockLibrary.FindTopMostDocumentLookup();
      this.Title = ""; //TODO What to assign to it http://itrserver/Bugs/BugDetail.aspx?bid=2910
    }
  }
}
