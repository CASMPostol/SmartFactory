using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  internal static class Extensions
  {
    public static ProductType ParseProductType(this string entry)
    {
      try
      {
        return (ProductType)Enum.Parse(typeof(Entities.ProductType), entry);
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
        return String.IsNullOrEmpty(entry) ? new Nullable<CompensationGood>() : (CompensationGood)Enum.Parse(typeof(Entities.CompensationGood), entry);
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
        throw new IPRDataConsistencyException("Extensions - GetTopMostDocumentLookup", msg, ex);
      }
    }
  }
}
