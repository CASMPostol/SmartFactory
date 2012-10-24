using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class DustExtension
  {
    internal static void ImportData( ConfigurationDustItem[] configuration, Entities edc )
    {
      List<Dust> list = new List<Dust>();
      foreach ( ConfigurationDustItem item in configuration )
      {
        Dust dst = new Dust
        {
          DustRatio = item.DustRatio,
          ProductType = item.ProductType.ParseProductType(),
        };
        list.Add( dst );
      };
      edc.Dust.InsertAllOnSubmit( list );
    }
  }
}
