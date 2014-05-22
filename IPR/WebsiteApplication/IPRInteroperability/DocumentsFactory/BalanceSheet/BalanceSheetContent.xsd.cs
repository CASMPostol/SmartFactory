using CAS.SharePoint.DocumentsFactory;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet
{
  public partial class BalanceSheetContent
  {
    /// <summary>
    /// Adds the document to collection.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>
    /// An object of <see cref="SPFile" /> containing the serialized <paramref name="destinationCollection" />
    /// </returns>
    public SPFile AddDocument2Collection( SPWeb site, string fileName, string listName)
    {
      return File.CreateXmlFile<BalanceSheetContent>(site, this, fileName, listName, DocumentNames.BalanceSheetContentName );
    }
    /// <summary>
    /// Updates the document.
    /// </summary>
    /// <param name="docFile">The doc file.</param>
    public void UpdateDocument( SPFile docFile )
    {
      File.WriteXmlFile<BalanceSheetContent>( docFile, this, DocumentNames.BalanceSheetContentName );
    }

  }
}
