using System;
using CAS.SmartFactory.Linq.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    internal void ExportPossible( double p, double? nullable, ActionResult _result )
    {
      if ( this.ProductType.Value != Linq.IPR.ProductType.IPRTobacco )
        return;
      //foreach ( var item in this. )
      //{
        
      //}
    }

  }
}
