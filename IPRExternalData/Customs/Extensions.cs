using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs
{
  public static class Extensions
  {
    /// <summary>
    /// Get requested customs procedure code 
    /// </summary>
    /// <param name="_cpc">The Customs Procedure Code.</param>
    /// <returns>Requested procedure code <see cref="CustomsProcedureCodes"/> - first two chars of the box 37</returns>
    public static CustomsProcedureCodes RequestedProcedure(this string _cpc)
    {
      switch (_cpc.Remove(2))
      {
        case "00":
          return CustomsProcedureCodes.NoProcedure;
        case "31":
          return CustomsProcedureCodes.ReExport;
        case "40":
          return CustomsProcedureCodes.FreeCirculation;
        case "51":
          return CustomsProcedureCodes.InwardProcessing;
        case "71":
          return CustomsProcedureCodes.CustomsWarehousingProcedure;
        default:
          throw new CustomsDataException("Extensions.RequestedProcedure", "Unsupported requested procedure");
      }
    }
  }
}
