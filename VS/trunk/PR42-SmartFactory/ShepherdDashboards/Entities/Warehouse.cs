using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Warehouse
  {
    internal static IQueryable<Warehouse> GatAll(EntitiesDataContext edc)
    {
      return from _idx in edc.Warehouse
             orderby _idx.Tytuł ascending
             select _idx;
    }
  }
}
