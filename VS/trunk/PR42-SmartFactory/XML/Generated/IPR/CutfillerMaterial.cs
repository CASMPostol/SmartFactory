using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.IPR
{
  public partial class CutfillerMaterial : Material
  {
    public override string GetMaterial()
    {
      return this.Material.Trim();
    }
    public override string GetMaterialDescription()
    {
      return this.MaterialDescription.Trim();
    }
  }
}
