using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class Ingredient
  {
    [Obsolete( "Use only for xml serializer" )]
    public Ingredient() { }
    
    public Ingredient( string tobaccoBatch, string tobaccoSKU, double quantity )
    {
      this.TobaccoBatch = tobaccoBatch;
      this.TobaccoQuantity = quantity;
      this.TobaccoSKU = tobaccoSKU;
    }
  }
}
