using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class CigarettesMaterial: Material
  {
    /// <summary>
    /// Gets the material.
    /// </summary>
    /// <returns></returns>
    public override string GetMaterial()
    {
      return this.Material;
    }
    /// <summary>
    /// Gets the material description.
    /// </summary>
    /// <returns></returns>
    public override string GetMaterialDescription()
    {
      return this.Material_Description;
    }
    /// <summary>
    /// Gets a value indicating whether this instance is menthol.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is menthol; otherwise, <c>false</c>.
    /// </value>
    public bool IsMenthol { get { return Menthol == null ? false : Menthol.StartsWith( "M" ); } }
  }
}
