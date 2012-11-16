using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;
using XmlConfiguration = CAS.SmartFactory.xml.Dictionaries.Configuration;

namespace CAS.SmartFactory.Management
{
  /// <summary>
  /// EntitiesExtensions - provides import functionality 
  /// </summary>
  internal static class DictionaryImport
  {
    #region public

    /// <summary>
    /// Imports the data.
    /// </summary>
    /// <param name="data">The data.</param>
    /// <param name="url">The URL.</param>
    /// <param name="progressChanged">The progress changed.</param>
    internal static void ImportData( XmlConfiguration data, string url, ProgressChangedEventHandler progressChanged )
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
          ImportData( data.Format, edc );
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
          progressChanged( null, new ProgressChangedEventArgs( progress++, "Settings" ) );
          edc.SubmitChanges();
          ImportData( data.Settings, edc );
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
          //TODO update to the current model.
          //External = item.External,
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
    private static void ImportData( ConfigurationFormatItem[] configurationFormatItem, Entities edc )
    {
      List<Format> list = new List<Format>();
      foreach ( ConfigurationFormatItem item in configurationFormatItem )
      {
        Format frmt = new Format
        {
          CigaretteLenght = item.CigaretteLenght,
          FilterLenght = item.FilterLenght,
          Title = item.Title
        };
        list.Add( frmt );
      };
      edc.Format.InsertAllOnSubmit( list );
    }
    private static void ImportData(ConfigurationSettingsItem[] configuraion, Entities edc)
    {
        List<Settings> list = new List<Settings>();
        foreach ( ConfigurationSettingsItem item in configuraion )
        {
            Settings stg = new Settings
            {
                Title = item.Title,
                KeyValue = item.KeyValue,
            };
            list.Add(stg);
        };
        edc.Settings.InsertAllOnSubmit (list) ;
    }
    private static ProductType ParseProductType( this string entry )
    {
      try
      {
        return (ProductType)Enum.Parse( typeof( ProductType ), entry );
      }
      catch ( Exception )
      {
        return ProductType.None;
      }
    }
    private static string ParseCompensationGood( this string entry )
    {
      return entry;
    }

    #endregion

  }
}

