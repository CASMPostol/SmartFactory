using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.CELINA.SAD
{
  public partial class SAD : CustomsDocument
  {
    #region CustomsDocument
    public override string GetNrWlasny()
    {
      return Zgloszenie.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return String.Empty;
    }
    public override int GoodsTableLength()
    {
      if (this.Zgloszenie.Towar == null)
        return 0;
      return Zgloszenie.Towar.Length;
    }
    public override string MessageRootName()
    {
      return "SAD";
    }
    public override GoodDescription this[int index]
    {
      get { return Zgloszenie.Towar[index]; }
    }
    #endregion
  }
}

