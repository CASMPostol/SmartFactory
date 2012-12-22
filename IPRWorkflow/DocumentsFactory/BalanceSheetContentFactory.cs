using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  internal class BalanceSheetContentFactory
  {
    internal static BalanceSheetContent CreateRequestContent( JSOXLib list, int documentNo, string documentName )
    {
      BalanceSheetContent _ret = new BalanceSheetContent()
      {
        DocumentDate = DateTime.Today.Date,
        DocumentNo = documentName,
        EndDate = list.SituationDate.Value,
        IPRStock = GetIPRStock(),
        JSOX = GetJSOX(),
        SituationAtDate = list.SituationDate.Value,
        StartDate = list.PreviousMonthDate.Value
      };
      return _ret;
    }

    private static JSOContent GetJSOX()
    {
      throw new NotImplementedException();
    }

    private static IPRStockContent GetIPRStock()
    {
      throw new NotImplementedException();
    }


  }
}
