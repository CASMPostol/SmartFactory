using CAS.SharePoint;

namespace CAS.SmartFactory.xml
{
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
    /// Gets the required document finished good export consignment number.
    /// </summary>
    /// <param name="documentName">Name of the document.</param>
    /// <returns></returns>
    public static int? GetRequiredDocumentFinishedGoodExportConsignmentNumber( string documentName, string pattern )
    {
      string _cleranceString = documentName.SPValidSubstring().GetFirstCapture( pattern, "NA" );
      return _cleranceString.String2Int();
    }
  }
}
