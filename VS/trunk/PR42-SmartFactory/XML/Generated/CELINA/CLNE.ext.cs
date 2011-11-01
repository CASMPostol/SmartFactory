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
    public override int GoodsTableLength()
    {
      return 0;
    }
    public override string MessageRootName()
    {
      return "CLNE";
    }
    public override GoodDescription this[int index]
    {
      get { return null; }
    }
    #endregion
  }
}
