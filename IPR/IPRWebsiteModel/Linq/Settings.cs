using System;
using System.Collections.Generic;
using System.Linq;

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
    GoodsDescriptionBatchPattern,
    /// <summary>
    /// Batch Number Pattern
    /// </summary>
    BatchNumberPattern,
    /// <summary>
    /// The finished goods export form file name
    /// </summary>
    FinishedGoodsExportFormFileName,
    /// <summary>
    /// The request for account clearence form file name
    /// </summary>
    RequestForAccountClearenceFormFileName,
    /// <summary>
    /// The jsox balance report form file name
    /// </summary>
    JsoxBalanceReportFormFileName,
    /// <summary>
    /// The clearance title format
    /// </summary>
    ClearanceTitleFormat
  }
  public partial class Settings
  {

    #region public
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
    /// <summary>
    /// Finisheds the name of the goods export form file.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    public static string FinishedGoodsExportFormFileName( Entities edc, int number )
    {
      return String.Format( GetParameter( edc, SettingsEntry.FinishedGoodsExportFormFileName ), number );
    }
    /// <summary>
    /// Requests the name of for account clearence document.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    public static string RequestForAccountClearenceDocumentName( Entities edc, int number )
    {
      return String.Format( GetParameter( edc, SettingsEntry.RequestForAccountClearenceFormFileName ), number );
    }
    /// <summary>
    /// Requests the name of for balance sheet document.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    public static string RequestForBalanceSheetDocumentName( Entities edc, int number )
    {
      return String.Format( GetParameter( edc, SettingsEntry.JsoxBalanceReportFormFileName ), number );
    }
    #endregion

    #region private
    private static Dictionary<SettingsEntry, string> m_DefaultSettings = new Dictionary<SettingsEntry, string>()
    {
       {SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern,  @"(?<=P\w*\b\st\w*\b\s)(\d{7})"},
       {SettingsEntry.GoodsDescriptionTobaccoNamePattern , @"\b(.*)(?=\sGRADE:)"},
       {SettingsEntry.GoodsDescriptionWGRADEPattern,  @"(?<=\WGRADE:)\W*\b(\w*)"},
       {SettingsEntry.GoodsDescriptionSKUPattern,  @"(?<=\WSKU:)\W*\b(\d*)"}, 
       {SettingsEntry.GoodsDescriptionBatchPattern,  @"(?<=\WBatch:)\W*\b(\d*)"},
       {SettingsEntry.BatchNumberPattern,   @"\b(000\d{7})"},
       {SettingsEntry.FinishedGoodsExportFormFileName,   "Proces technologiczny {0:D7}"},
       {SettingsEntry.RequestForAccountClearenceFormFileName,   "Bilans {0:D7}"},
       {SettingsEntry.JsoxBalanceReportFormFileName,   "Wniosek o zamknięcie {0:D7}"},
       {SettingsEntry.ClearanceTitleFormat,   "Nr: {4:D5} {0}/{1} SAD: {2} ilość: {3:F2} kg"}
    };
    #endregion

  }
}
