using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.CELINA.CLNE
{
  public partial class CLNE : CustomsDocument
  {
    #region CustomsDocument
    public override string GetNrWlasny()
    {
      return this.Przyjecie.NrWlasny;
    }
    public override string GetReferenceNumber()
    {
      return this.Przyjecie.NrCelina;
    }

    public override decimal GetItemNo(int index)
    {
      throw new NotImplementedException();
    }
    public override int GoodsTableLength()
    {
      return 0;
    }
    #endregion
  }
}
