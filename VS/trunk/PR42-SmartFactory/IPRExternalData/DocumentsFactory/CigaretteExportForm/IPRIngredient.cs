using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class IPRIngredient
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRIngredient"/> class.
    /// </summary>
    [Obsolete( "Use only for xml serializer" )]
    public IPRIngredient() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRIngredient"/> class.
    /// </summary>
    /// <param name="tobaccoBatch">The tobacco batch.</param>
    /// <param name="tobaccoSKU">The tobacco SKU.</param>
    /// <param name="quantity">The quantity.</param>
    public IPRIngredient( string tobaccoBatch, string tobaccoSKU, double quantity ) :
      base( tobaccoBatch, tobaccoSKU, quantity )
    { }
  }
}
