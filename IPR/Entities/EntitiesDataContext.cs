using System;
using Microsoft.SharePoint.Linq;
using XmlConfiguration = CAS.SmartFactory.xml.Dictionaries.Configuration;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class EntitiesDataContext
  {
    /// <summary>
    /// Persists to the content database changes made by the current user to one or more lists using the specified failure mode;
    /// or, if a concurrency conflict is found, populates the <see cref="P:Microsoft.SharePoint.Linq.DataContext.ChangeConflicts"/> property.
    /// </summary>
    /// <param name="mode">Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it 
    /// finds that a list item has been changed by another process since it was retrieved.
    /// </param>
    public void SubmitChangesSilently(RefreshMode mode)
    {
      try
      {
        SubmitChanges();
      }
      catch (ChangeConflictException)
      {
        foreach (ObjectChangeConflict changedListItem in this.ChangeConflicts)
        {
          changedListItem.Resolve(mode);
        }
        this.SubmitChanges();
      }
      catch (Exception)
      {
      }// end catch
    }
    public static void ImportData(XmlConfiguration data, string url)
    {
      EntitiesDataContext edc = null;
      try
      {
        edc = new EntitiesDataContext(url);
        Entities.Format.ImportData(data.Format, edc);
        edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
        Entities.Consent.ImportData(data.Consent, edc);
        Entities.CustomsUnion.ImportData(data.CustomsUnion, edc);
        Entities.CutfillerCoefficient.ImportData(data.CutfillerCoefficient, edc);
        Entities.Dust.ImportData(data.Dust, edc);
        Entities.PCNCode.ImportData(data.PCNCode, edc);
        Entities.SHMenthol.ImportData(data.SHMenthol, edc);
        Entities.Usage.ImportData(data.Usage, edc);
        Entities.Warehouse.ImportData(data.Warehouse, edc);
        Entities.Waste.ImportData(data.Waste, edc);
        edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
      }
      finally
      {
        if (edc != null)
        {
          edc.Dispose();
        }
      }
    }
  }
}

