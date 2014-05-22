using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class RegularIngredient
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RegularIngredient"/> class.
    /// </summary>
    [Obsolete()]
    public RegularIngredient() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="RegularIngredient"/> class.
    /// </summary>
    /// <param name="tobaccoBatch">The tobacco batch.</param>
    /// <param name="tobaccoSKU">The tobacco SKU.</param>
    /// <param name="quantity">The quantity od tobacco.</param>
    public RegularIngredient( string tobaccoBatch, string tobaccoSKU, double quantity ) :
      base( tobaccoBatch, tobaccoSKU, quantity )
    { }
  }
}
