using System;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.DustWasteForm
{
  public partial class DocumentContent
  {
    /// <summary>
    /// Compensatiion good enum
    /// </summary>
    public enum CompensatiionGood { Dust, Waste }
    /// <summary>
    /// Adds the document to collection <see cref="SPFileCollection" />.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="compensatiionGood">The compensatiion good.</param>
    /// <returns>
    /// An object of <see cref="SPFile" /> containing the serialized <paramref name="destinationCollection" />
    /// </returns>
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName, CompensatiionGood compensatiionGood )
    {
      string stylesheetName = String.Empty;
      switch ( compensatiionGood )
      {
        case CompensatiionGood.Dust:
          stylesheetName = DocumentNames.DustFormStylesheetName;
          break;
        case CompensatiionGood.Waste:
          stylesheetName = DocumentNames.WasteFormStylesheetName;
          break;
      }
      return DocumentNames.CreateXmlFile<DocumentContent>( destinationCollection, fileName, this, stylesheetName );
    }
  }
}
