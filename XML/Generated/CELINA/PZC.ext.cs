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
    #region MyRegion
    public override string GetNrWlasny()
    {
      return this.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return this.ZwolnienieDoProcedury.NrCelina;
    }
    #endregion

    public override decimal GetItemNo(int index)
    {
      return this.ZwolnienieDoProcedury.Towar[index].PozId;
    }
    public override int GoodsTableLength()
    {
      return this.ZwolnienieDoProcedury.Towar.Length;
    }
  }
}
