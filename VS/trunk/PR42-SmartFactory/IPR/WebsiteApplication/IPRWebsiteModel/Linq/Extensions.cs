using System;
using System.Linq;
using CAS.SmartFactory.IPR;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  internal static class LinqExtensions
  {
    public static ProductType ParseProductType( this string entry )
    {
      try
      {
        return (ProductType)Enum.Parse( typeof( Linq.ProductType ), entry );
      }
      catch ( Exception )
      {
        return ProductType.None;
      }
    }
  }
}
