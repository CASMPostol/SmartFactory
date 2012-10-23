using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SKUCutfiller
  {
    /// <summary>
    /// Gets the IPR material.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    protected override bool? GetIPRMaterial(Entities edc)
    {
      return (!String.IsNullOrEmpty(BlendPurpose)) && BlendPurpose.Contains("NEU");
    }
  }
}
