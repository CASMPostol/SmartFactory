using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;
using XmlConfiguration = CAS.SmartFactory.xml.Dictionaries.Configuration;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// EntitiesExtensions - provides import functionality 
  /// </summary>
  public static class EntitiesExtensions
  {
    #region public

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
    public static void ImportData( XmlConfiguration data, string url, ProgressChangedEventHandler progressChanged )
    {
      Entities edc = null;
      int progress = 0;
      try
      {
        using ( Entities _this = new Entities( url ) )
        {
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Connecting to website" ) );
          edc = new Entities( url );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Format" ) );
          FormatExtension.ImportData( data.Format, edc );
          edc.SubmitChanges();
          //edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Consent" ) );
          ImportData( data.Consent, edc );
          edc.SubmitChanges();
          progressChanged( null, new ProgressChangedEventArgs( progress++, "CustomsUnion" ) );
          ImportData( data.CustomsUnion, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "CutfillerCoefficient" ) );
          edc.SubmitChanges();
          ImportData( data.CutfillerCoefficient, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Dust" ) );
          edc.SubmitChanges();
          ImportData( data.Dust, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "PCNCode" ) );
          edc.SubmitChanges();
          ImportData( data.PCNCode, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "SHMenthol" ) );
          edc.SubmitChanges();
          ImportData( data.SHMenthol, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Usage" ) );
          edc.SubmitChanges();
          ImportData( data.Usage, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Warehouse" ) );
          edc.SubmitChanges();
          ImportData( data.Warehouse, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Waste" ) );
          edc.SubmitChanges();
          ImportData( data.Waste, edc );
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Submiting Changes" ) );
          edc.SubmitChanges();
          //edc.SubmitChangesSilently( RefreshMode.OverwriteCurrentValues );
        }
      }
      finally
      {
        if ( edc != null )
        {
          edc.Dispose();
        }
      }
    }

    #endregion

    #region private

    private static void ImportData( ConfigurationConsentItem[] configuration, Entities edc )
    {
      List<Consent> list = new List<Consent>();
      foreach ( ConfigurationConsentItem item in configuration )
      {
        Consent cns = new Consent
        {
          ProductivityRateMax = item.ProductivityRateMax,
          ProductivityRateMin = item.ProductivityRateMin,
          ValidFromDate = item.ValidFromDate,
          ValidToDate = item.ValidToDate,
          ConsentPeriod = item.ConsentPeriod,
          Title = item.ConsentNo
        };
        list.Add( cns );
      };
      edc.Consent.InsertAllOnSubmit( list );
    } //ImportData
    private static void ImportData( ConfigurationCustomsUnionItem[] configuration, Entities edc )
    {
      List<CustomsUnion> list = new List<CustomsUnion>();
      foreach ( ConfigurationCustomsUnionItem item in configuration )
      {
        CustomsUnion csu = new CustomsUnion
        {
          EUPrimeMarket = item.EUPrimeMarket,
          Title = item.Title
        };
        list.Add( csu );
      };
      edc.CustomsUnion.InsertAllOnSubmit( list );
    } //ImportData
    private static void ImportData( ConfigurationCutfillerCoefficientItem[] configuration, Entities edc )
    {
      List<CutfillerCoefficient> list = new List<CutfillerCoefficient>();
      foreach ( ConfigurationCutfillerCoefficientItem item in configuration )
      {
        CutfillerCoefficient cc = new CutfillerCoefficient
        {
          CFTProductivityRateMax = item.CFTProductivityRateMax,
          CFTProductivityRateMin = item.CFTProductivityRateMin
        };
        list.Add( cc );
      };
      edc.CutfillerCoefficient.InsertAllOnSubmit( list );
    }
    private static void ImportData( ConfigurationDustItem[] configuration, Entities edc )
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
    private static void ImportData( ConfigurationPCNCodeItem[] configuration, Entities edc )
    {
      List<PCNCode> list = new List<PCNCode>();
      foreach ( ConfigurationPCNCodeItem item in configuration )
      {
        PCNCode pcn = new PCNCode
        {
          CompensationGood = item.CompensationGood.ParseCompensationGood(),
          ProductCodeNumber = item.ProductCodeNumber,
          Title = item.Title
        };
        list.Add( pcn );
      };
      edc.PCNCode.InsertAllOnSubmit( list );
    }
    private static void ImportData( ConfigurationSHMentholItem[] configuration, Entities edc )
    {
      List<SHMenthol> list = new List<SHMenthol>();
      foreach ( ConfigurationSHMentholItem item in configuration )
      {
        SHMenthol shm = new SHMenthol
        {
          ProductType = item.ProductType.ParseProductType(),
          SHMentholRatio = item.SHMentholRatio,
        };
        list.Add( shm );
      };
      edc.SHMenthol.InsertAllOnSubmit( list );
    }
    private static void ImportData( ConfigurationUsageItem[] configuration, Entities edc )
    {
      List<Usage> list = new List<Usage>();
      foreach ( ConfigurationUsageItem item in configuration )
      {
        Usage usg = new Usage
        {
          Batch = null,
          FormatIndex = Format.GetFormatLookup( item.Format_lookup, edc ),
          UsageMax = item.UsageMax,
          UsageMin = item.UsageMin,
          //TODO  [pr4-3560] Usage List - add data and display CFT... column http://itrserver/Bugs/BugDetail.aspx?bid=3560
          //Schema does not contain definition of this columns.
          CTFUsageMax = int.MaxValue,
          CTFUsageMin = int.MinValue
        };
        list.Add( usg );
      };
      edc.Usage.InsertAllOnSubmit( list );
    }
    private static void ImportData( ConfigurationWarehouseItem[] configuration, Entities edc )
    {
      List<Warehouse> list = new List<Warehouse>();
      foreach ( ConfigurationWarehouseItem item in configuration )
      {
        Warehouse wrh = new Warehouse
        {
          External = item.External,
          ProductType = item.ProductType.ParseProductType(),
          Title = item.Title
        };
        list.Add( wrh );
      };
      edc.Warehouse.InsertAllOnSubmit( list );
    }
    private static void ImportData( ConfigurationWasteItem[] configuration, Entities edc )
    {
      List<Waste> list = new List<Waste>();
      foreach ( ConfigurationWasteItem item in configuration )
      {
        Waste wst = new Waste
        {
          Batch = null,
          ProductType = item.ProductType.ParseProductType(),
          WasteRatio = item.WasteRatio
        };
        list.Add( wst );
      };
      edc.Waste.InsertAllOnSubmit( list );
    }

    #endregion

  }
}

