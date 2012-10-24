using System;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class Extensions
  {
    public static ProductType ParseProductType(this string entry)
    {
      try
      {
        return (ProductType)Enum.Parse(typeof(ProductType), entry);
      }
      catch (Exception)
      {
        return ProductType.None;
      }
    }
    public static CompensationGood? ParseCompensationGood(this string entry)
    {
      try
      {
        return String.IsNullOrEmpty( entry ) ? new Nullable<CompensationGood>() : (CompensationGood)Enum.Parse( typeof( CompensationGood ), entry );
      }
      catch (Exception)
      {
        return CompensationGood.Invalid;
      }
    }
    /// <summary>
    /// Gets the top most document lookup.
    /// </summary>
    /// <param name="library">The library.</param>
    /// <returns></returns>
    public static Dokument GetTopMostDocumentLookup(this  EntityList<Dokument> library)
    {
      try
      {
        return (from idx in library orderby idx.Identyfikator descending select idx).First();
      }
      catch (Exception ex)
      {
        string msg = "Cannot find a library";
        throw new IPRDataConsistencyException("Extensions - GetTopMostDocumentLookup", msg, ex, "Dokument lookup error");
      }
    }
  }
}
