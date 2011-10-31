using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.CELINA.SAD
{
  public partial class SAD : CustomsDocument
  {

    #region MyRegion
    public override string GetNrWlasny()
    {
      return Zgloszenie.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return String.Empty;
    }
    public override decimal GetItemNo(int index)
    {
      return Zgloszenie.Towar[index].PozId;
    }
    public override int GoodsTableLength()
    {
      return Zgloszenie.Towar.Length;
    }
    #endregion
  }
}

