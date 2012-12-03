using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Warehouse
  {
    public static Warehouse Find( Entities edc, string index )
    {
      return (from Warehouse item in edc.Warehouse where item.Title.Contains(index) select item).FirstOrDefault<Warehouse>();
    }
  }
}
