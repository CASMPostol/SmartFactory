using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.ECS.IE529
{
	public partial class IE529: CustomsDocument
	{

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
        return Zwolnienie.Towar.Length;
      }
      public override decimal GetItemNo(int index)
      {
       return Zwolnienie.Towar[index].Nr;
      }
    }
}
