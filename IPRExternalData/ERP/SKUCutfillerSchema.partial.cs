using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class Cutfiller : SKU
  {
    public override SKU.SKUType Type
    {
      get { return SKUType.Cutfiller; }
    }
    public override Material[] GetMaterial()
    {
      return this.Material;
    }
  }
}

