using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class BatchMaterial
  {
    public bool IsFinishedGood
    {
      get
      {
        string _dsc = this.Material_description.Trim().ToUpper();
        return _dsc.Contains( "SKU" ) || _dsc.Contains( "CFT" );
      }
    }
  }
}
