using System;
using CAS.SharePoint;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.xml
{
  //TODO  [pr4-3550] Add localization to the application http://itrserver/Bugs/BugDetail.aspx?bid=3550
  /// <summary>
  /// A collection of key used ro select strings from resources 
  /// </summary>
  public static class XMLResources
  {
    //TODO  must be assigned.
    /// <summary>
    /// Required document finishedgood export consignmen tcode
    /// </summary>
    public const string RequiredDocumentConsignmentCode = "9DK8";
    /// <summary>
    /// Finisheds the name of the goods export form file.
    /// </summary>
    /// <param name="number">The number.</param>
    /// <returns></returns>
    public static string FinishedGoodsExportFormFileName( int number )
    {
      return String.Format( m_FinishedGoodsExportFormFileName.GetLocalizedString(), number );
    }
    /// <summary>
    /// Gets the required document finished good export consignment number.
    /// </summary>
    /// <param name="documentName">Name of the document.</param>
    /// <returns></returns>
    public static int? GetRequiredDocumentFinishedGoodExportConsignmentNumber( string documentName, string pattern )
    {
      string _cleranceString = documentName.SPValidSubstring().GetFirstCapture( pattern, "NA" );
      return _cleranceString.String2Int();
    }
    public static string RequestForAccountClearenceDocumentName( int number )
    {
      return String.Format( m_RequestForAccountClearenceDocumentName.GetLocalizedString(), number );
    }
    public static string RequestForBalanceSheetDocumentName( int number )
    {
      return String.Format( m_RequestForAccountClearenceDocumentName.GetLocalizedString(), number );
    }
    private const string m_FinishedGoodsExportFormFileName = "Proces technologiczny {0:D7}";
    private const string m_RequestForAccountClearenceDocumentName = "Account Clearence Application {0:D7}";
    private const string m_RequestForBalanceSheetCollectionDocumentName = "Balance Sheet {0:D7}";
  }
}
