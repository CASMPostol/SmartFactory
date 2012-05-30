using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class CigarettesMaterial : Material
  {
    public override string GetMaterial()
    {
      return this.Material;
    }
    public override string GetMaterialDescription()
    {
      return this.Material_Description;
    }
  }
}
