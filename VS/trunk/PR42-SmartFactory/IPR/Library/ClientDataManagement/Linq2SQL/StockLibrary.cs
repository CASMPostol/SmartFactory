//<summary>
//  Title   : public partial class StockLibrary
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  public partial class StockLibrary
  {

    internal static void MarkSQLOnly(IPRDEV _sqledc, int id)
    {
      StockLibrary _se = _sqledc.StockLibrary.Where<StockLibrary>(x => x.ID == id).FirstOrDefault<StockLibrary>();
      if (_se == null)
        return;
      _se.OnlySQL = true;
    }
  }
}
