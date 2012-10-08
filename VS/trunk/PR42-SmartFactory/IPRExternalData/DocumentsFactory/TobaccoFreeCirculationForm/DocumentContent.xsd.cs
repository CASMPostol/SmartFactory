using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.TobaccoFreeCirculationForm
{
  public partial class DocumentContent
  {
    /// <summary>
    /// Adds the document to collection <see cref="SPFileCollection"/>.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>An object of <see cref="SPFile"/> containing the serialized <paramref name="destinationCollection"/></returns>
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName )
    {
      return DocumentNames.CreateXmlFile<DocumentContent>( destinationCollection, fileName, this, DocumentNames.TobaccoFormStylesheetName);
    }
  }
}
