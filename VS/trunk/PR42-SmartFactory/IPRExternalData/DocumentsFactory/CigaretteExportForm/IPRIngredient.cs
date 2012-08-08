using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class IPRIngredient
  {
    public IPRIngredient( string tobaccoBatch, string tobaccoSKU, double quantity ) :
      base( tobaccoBatch, tobaccoSKU, quantity )
    { }

  }
}
