using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm
{
  public partial class DocumentContent
  {
    /// <summary>
    /// Adds the document to collection <see cref="SPFileCollection"/>.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>An object of <see cref="SPFile"/> containing the serialized <paramref name="destinationCollection"/></returns>
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName, string stylesheetName )
    {
      return DocumentNames.CreateXmlFile<DocumentContent>( destinationCollection, fileName, this, stylesheetName );
    }
    /// <summary>
    /// DustForm Stylesheet Name
    /// </summary>
    public const string DustFormStylesheetName = "DustFormStylesheet.xslt";
    /// <summary>
    /// WasteForm Stylesheet Name
    /// </summary>
    public const string WasteFormStylesheetName = "DustFormStylesheet.xslt";
  }

}
