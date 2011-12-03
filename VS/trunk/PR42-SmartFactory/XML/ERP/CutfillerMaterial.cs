using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class CutfillerMaterial : Material
  {
    public override string GetMaterial()
    {
      return this.Material;
    }
    public override string GetMaterialDescription()
    {
      return this.MaterialDescription;
    }
  }
}
