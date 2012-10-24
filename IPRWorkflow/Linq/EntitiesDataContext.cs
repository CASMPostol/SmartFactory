using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using XmlConfiguration = CAS.SmartFactory.xml.Dictionaries.Configuration;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class EntitiesExtensions
  {
    ///// <summary>
    ///// Persists to the content database changes made by the current user to one or more lists using the specified failure mode;
    ///// or, if a concurrency conflict is found, populates the <see cref="P:Microsoft.SharePoint.Linq.DataContext.ChangeConflicts"/> property.
    ///// </summary>
    ///// <param name="mode">Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it 
    ///// finds that a list item has been changed by another process since it was retrieved.
    ///// </param>
    //public void SubmitChangesSilently(RefreshMode mode)
    //{
    //  try
    //  {
    //    SubmitChanges();
    //  }
    //  catch (ChangeConflictException)
    //  {
    //    foreach (ObjectChangeConflict changedListItem in this.ChangeConflicts)
    //    {
    //      changedListItem.Resolve(mode);
    //    }
    //    this.SubmitChanges();
    //  }
      //catch (Exception)
      //{
      //}// end catch
    //}
    internal static void ImportData( this Entities _this, XmlConfiguration data, string url, ProgressChangedEventHandler progressChanged )
    {
      Entities edc = null;
      int progress = 0;
      try
      {
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Connecting to website"));
        edc = new Entities(url);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Format"));
        Linq.IPR.FormatExtension.ImportData( data.Format, edc );
        edc.SubmitChanges();
        //edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Consent"));
        Linq.IPR.ConsentExtension.ImportData( data.Consent, edc );
        edc.SubmitChanges();
        progressChanged( null, new ProgressChangedEventArgs( progress++, "CustomsUnion" ) );
        Linq.IPR.CustomsUnionExtension.ImportData( data.CustomsUnion, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "CutfillerCoefficient"));
        edc.SubmitChanges();
        Linq.IPR.CutfillerCoefficientExtension.ImportData( data.CutfillerCoefficient, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Dust"));
        edc.SubmitChanges();
        Linq.IPR.DustExtension.ImportData( data.Dust, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "PCNCode"));
        edc.SubmitChanges();
        Linq.IPR.PCNCodeExtension.ImportData( data.PCNCode, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "SHMenthol"));
        edc.SubmitChanges();
        Linq.IPR.SHMentholExtension.ImportData( data.SHMenthol, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Usage"));
        edc.SubmitChanges();
        Linq.IPR.UsageExtension.ImportData( data.Usage, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Warehouse"));
        edc.SubmitChanges();
        Linq.IPR.WarehouseExtension.ImportData( data.Warehouse, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Waste"));
        edc.SubmitChanges();
        Linq.IPR.WasteExtension.ImportData( data.Waste, edc );
        progressChanged(null, new ProgressChangedEventArgs(progress++, "Submiting Changes"));
        edc.SubmitChanges();
        //edc.SubmitChangesSilently( RefreshMode.OverwriteCurrentValues );
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

