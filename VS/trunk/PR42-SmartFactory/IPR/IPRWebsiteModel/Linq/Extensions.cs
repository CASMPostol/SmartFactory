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
    //TODO to be removed
    //public static CompensationGood? ParseCompensationGood(this string entry)
    //{
    //  try
    //  {
    //    return String.IsNullOrEmpty( entry ) ? new Nullable<CompensationGood>() : (CompensationGood)Enum.Parse( typeof( Linq.CompensationGood ), entry );
    //  }
    //  catch (Exception)
    //  {
    //    return CompensationGood.Invalid;
    //  }
    //}
    ///// <summary>
    ///// Gets the top most document lookup.
    ///// </summary>
    ///// <param name="library">The library.</param>
    ///// <returns></returns>
    //public static Dokument FindTopMostDocumentLookup( this  EntityList<Dokument> library )
    //{
    //  return ( from idx in library orderby idx.Id descending select idx ).FirstOrDefault();
    //}
  }
}
