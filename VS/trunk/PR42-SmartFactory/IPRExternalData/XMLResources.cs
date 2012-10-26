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
    public static int? GetRequiredDocumentFinishedGoodExportConsignmentNumber( string documentName )
    {
      int? _cleranceInt = new Nullable<int>();
      try
      {
        string _cleranceString = documentName.SPValidSubstring().GetFirstCapture( XMLResources.m_RequiredDocumentFinishedGoodExportConsignmentPattern );
        _cleranceInt = _cleranceString.String2Int();
      }
      catch ( GenericStateMachineEngine.ActionResult ) { }
      return _cleranceInt;
    }
    private const string m_FinishedGoodsExportFormFileName = "Proces technologiczny {0:D7}";
    private const string m_RequiredDocumentFinishedGoodExportConsignmentPattern = @"(?<=P\w*\b\st\w*\b\s)(\d{7})";
  }
}
