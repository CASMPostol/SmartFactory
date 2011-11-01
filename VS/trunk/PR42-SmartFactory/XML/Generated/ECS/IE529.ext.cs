using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.ECS.IE529
{
  public partial class IE529 : CustomsDocument
  {
    #region CustomsDocument
    public override string GetNrWlasny()
    {
      return this.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return this.Zwolnienie.MRN;
    }
    public override int GoodsTableLength()
    {
      if (this.Zwolnienie.Towar == null)
        return 0;
      return Zwolnienie.Towar.Length;
    }
    public override string MessageRootName()
    {
      return "IE529";
    }
    public override GoodDescription this[int index]
    {
      get { return Zwolnienie.Towar[index]; }
    }
    #endregion
  }
}
