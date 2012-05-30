using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  
  public partial class Cigarettes : SKU
  {
    public override SKU.SKUType Type
    {
      get { return SKUType.Cigarettes; }
    }
    public override Material[] GetMaterial()
    {
      return Material;
    }
  }
}
