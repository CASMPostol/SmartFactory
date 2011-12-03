using System;
using Microsoft.SharePoint.Linq;
using XmlConfiguration = CAS.SmartFactory.xml.Dictionaries.Configuration;
using System.ComponentModel;

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
      //catch (Exception)
      //{
      //}// end catch
    }
    public static void ImportData(XmlConfiguration data, string url, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      int progress = 0;
      try
      {
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Format"));
        edc = new EntitiesDataContext(url);
        Entities.Format.ImportData(data.Format, edc);
        edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Consent"));   
        Entities.Consent.ImportData(data.Consent, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "CustomsUnion"));
        Entities.CustomsUnion.ImportData(data.CustomsUnion, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "CutfillerCoefficient"));
        Entities.CutfillerCoefficient.ImportData(data.CutfillerCoefficient, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Dust"));
        Entities.Dust.ImportData(data.Dust, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "PCNCode"));
        Entities.PCNCode.ImportData(data.PCNCode, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "SHMenthol"));
        Entities.SHMenthol.ImportData(data.SHMenthol, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Usage"));
        Entities.Usage.ImportData(data.Usage, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Warehouse"));
        Entities.Warehouse.ImportData(data.Warehouse, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Waste"));
        Entities.Waste.ImportData(data.Waste, edc);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Submiting Changes"));
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

