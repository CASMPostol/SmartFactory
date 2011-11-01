using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.CELINA.PZC
{
  /// <summary>
  /// 
  /// </summary>
  public partial class PZC : CustomsDocument
  {
    #region CustomsDocument
    public override string GetNrWlasny()
    {
      return this.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return this.ZwolnienieDoProcedury.NrCelina;
    }
    public override int GoodsTableLength()
    {
      if (this.ZwolnienieDoProcedury.Towar == null)
        return 0;
      return this.ZwolnienieDoProcedury.Towar.Length;
    }
    public override string MessageRootName()
    {
      return "PZC";
    }
    public override GoodDescription this[int index]
    {
      get { return ZwolnienieDoProcedury.Towar[index]; }
    }
    #endregion
  }
}
