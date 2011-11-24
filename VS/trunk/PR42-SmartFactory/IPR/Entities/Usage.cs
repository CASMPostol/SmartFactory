using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Usage
  {

    internal static Usage GetLookup(Format format, EntitiesDataContext edc)
    {
      Usage value = null;
      try
      {
        value = (from idx in edc.Usage where idx.FormatLookup.Identyfikator == format.Identyfikator select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
        return value;
      }
      catch (ArgumentNullException)
      {
        //value = new Usage() {   = 0, ProductType = type, Tytuł = type.ToString() }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }
  }
}
