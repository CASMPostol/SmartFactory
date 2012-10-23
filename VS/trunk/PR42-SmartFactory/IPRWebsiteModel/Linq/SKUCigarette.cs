using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SKUCigarette
  {
    #region public
    #endregion

    #region private
    /// <summary>
    /// Gets the IPR material.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    protected override bool? GetIPRMaterial(Entities edc)
    {

      if (String.IsNullOrEmpty(this.PrimeMarket))
      {
        Anons.WriteEntry(edc, m_Source, string.Format(m_PMTemplate, this.Title));
        return new Nullable<bool>();
      }
      return !CustomsUnion.CheckIfUnion(this.PrimeMarket, edc);
    }
    #endregion
    private const string m_Source = "Cigarettes SKU processing";
    private const string m_PMTemplate = "I cannot analize the market for {0} because the name is epty";
  }
}
