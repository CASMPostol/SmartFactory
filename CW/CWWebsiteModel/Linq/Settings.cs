using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Settings entity class
  /// </summary>
  public partial class Settings
  {
    /// <summary>
    /// Gets the parameter.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The parameter index.</param>
    /// <returns></returns>
    public static string GetParameter( Entities edc, SettingsEntry index )
    {
      Settings _ret = ( from _sx in edc.Settings where _sx.KeyValue.Contains( index.ToString() ) select _sx ).FirstOrDefault();
      if ( _ret == null )
      {
        _ret = new Settings()
        {
          KeyValue = index.ToString(),
          Title = m_DefaultSettings[ index ]
        };
        edc.Settings.InsertOnSubmit( _ret );
        edc.SubmitChanges();
      }
      return _ret.Title;
    }

    public static string CustomsProcedureCodeA004 = "A004";
    public static string CustomsProcedureCodeN865 = "N865";
    public static string CustomsProcedureCodeN954 = "N954";

    private static Dictionary<SettingsEntry, string> m_DefaultSettings = new Dictionary<SettingsEntry, string>()
    {
       {SettingsEntry.GoodsDescription_CWPackageKg_Pattern,  @"(?<=P\w*\b\st\w*\b\s)(\d{7})"},
       {SettingsEntry.GoodsDescription_CWQuantity_CWPackageKg_Pattern , @"\b(.*)(?=\sGRADE:)"},
       {SettingsEntry.GoodsDescription_CWQuantity_Pattern,  @"(?<=\WGRADE:)\W*\b(\w*)"},
       {SettingsEntry.GoodsDescription_Units_Pattern,  @"(?<=\WSKU:)\W*\b(\d*)"}, 
       {SettingsEntry.GoodsDescription_CertificateOfAuthenticity_Pattern,  @"(?<=\WBatch:)\W*\b(\d*)"},
       {SettingsEntry.DefaultValidToDatePeriod, "720"},
       {SettingsEntry.GoodsDescription_CertificateOfOrgin_Pattern, "Proces technologiczny {0:D7}"},
       {SettingsEntry.LooselyFormatedDate, @"(?<=/)\D*(\d{1,2}).(\d{1,2}).(\d{4})"},
    };
  }
  public enum SettingsEntry
  {
    GoodsDescription_CWPackageKg_Pattern,
    GoodsDescription_CWQuantity_CWPackageKg_Pattern,
    GoodsDescription_CWQuantity_Pattern,
    GoodsDescription_Units_Pattern,
    GoodsDescription_CertificateOfAuthenticity_Pattern,
    DefaultValidToDatePeriod,
    GoodsDescription_CertificateOfOrgin_Pattern,
    LooselyFormatedDate
  }
}
