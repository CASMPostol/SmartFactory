using System.Collections.Generic;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public static class WasteExtension
  {
    #region public

    internal static void ImportData(ConfigurationWasteItem[] configuration, Entities edc)
    {
      List<Waste> list = new List<Waste>();
      foreach (ConfigurationWasteItem item in configuration)
      {
        Waste wst = new Waste
        {
          Batch = null,
          ProductType = item.ProductType.ParseProductType(),
          WasteRatio = item.WasteRatio
        };
        list.Add(wst);
      };
      edc.Waste.InsertAllOnSubmit(list);
    } 
    #endregion
  }
}
