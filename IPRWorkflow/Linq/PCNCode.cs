using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class PCNCodeExtension
  {
    internal static void ImportData(ConfigurationPCNCodeItem[] configuration, Entities edc)
    {
      List<PCNCode> list = new List<PCNCode>();
      foreach (ConfigurationPCNCodeItem item in configuration)
      {
        PCNCode pcn = new PCNCode
        {
          CompensationGood = item.CompensationGood.ParseCompensationGood(),
          ProductCodeNumber = item.ProductCodeNumber,
          Title = item.Title
        };
        list.Add(pcn);
      };
      edc.PCNCode.InsertAllOnSubmit(list);
    }
  }
}
