using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// SettingsEntry
  /// </summary>
  public enum SettingsEntry
  {
    /// <summary>
    /// The required document finished good export consignment pattern
    /// </summary>
    RequiredDocumentFinishedGoodExportConsignmentPattern,
    /// <summary>
    /// The goods description tobacco name pattern
    /// </summary>
    GoodsDescriptionTobaccoNamePattern,
    /// <summary>
    /// The goods description GRADE pattern
    /// </summary>
    GoodsDescriptionWGRADEPattern,
    /// <summary>
    /// The goods description SKU pattern
    /// </summary>
    GoodsDescriptionSKUPattern,
    /// <summary>
    /// The goods description batch pattern
    /// </summary>
    GoodsDescriptionBatchPattern
  }
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
      Settings _ret = ( from _sx in edc.Settings where _sx.Title == index.ToString() select _sx ).FirstOrDefault();
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
      return _ret.KeyValue;
    }
    private static Dictionary<SettingsEntry, string> m_DefaultSettings = new Dictionary<SettingsEntry, string>()
    {
    {SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern,  @"(?<=P\w*\b\st\w*\b\s)(\d{7})"},
     {SettingsEntry.GoodsDescriptionTobaccoNamePattern , @"\b(.*)(?=\sGRADE:)"},
     {SettingsEntry.GoodsDescriptionWGRADEPattern,  @"(?<=\WGRADE:)\W*\b(\w*)"},
     {SettingsEntry.GoodsDescriptionSKUPattern,  @"(?<=\WSKU:)\W*\b(\d*)"}, 
     {SettingsEntry.GoodsDescriptionBatchPattern,  @"(?<=\WBatch:)\W*\b(\d*)"}
    };
  }
}
