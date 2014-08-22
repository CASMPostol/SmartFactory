using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  public partial class StockEntry
  {

    internal static void MarkSQLOnly(IPRDEV _sqledc, int id)
    {
      StockEntry _se = _sqledc.StockEntry.Where<StockEntry>(x => x.ID == id).FirstOrDefault<StockEntry>();
      if (_se == null)
        return;
      _se.OnlySQL = true;
    }
  }
}
