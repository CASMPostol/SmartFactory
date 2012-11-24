using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.erp
{
  public partial class BatchMaterial
  {
    public bool IsFinishedGood { get { return this.Material_description.Trim().ToUpper().Contains( "SKU" ); } }
  }
}
