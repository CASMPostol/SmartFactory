using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Waste
  {
    #region public
    internal static Waste GetLookup(ProductType type, Entities edc)
    {
      try
      {
        return (from idx in edc.Waste where idx.ProductType == type orderby idx.Wersja descending select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_Message, type), ex, "Waste lookup error");
      }
    }
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

    #region private
    private const string m_Source = "Waste";
    private const string m_Message = "I cannot find waste coefficient for the product type: {0}";
    #endregion
  }
}
