using CAS.SharePoint.DocumentsFactory;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.AccountClearance
{
  public partial class RequestContent
  {
    /// <summary>
    /// Adds the document to collection.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>
    /// An object of <see cref="SPFile" /> containing the serialized <paramref name="destinationCollection" />
    /// </returns>
    public SPFile AddDocument2Collection( SPWeb site, string fileName, string listName )
    {
      return File.CreateXmlFile<RequestContent>( site, this, fileName, listName, DocumentNames.RequestForAccountClearenceName );
    }

  }
}
