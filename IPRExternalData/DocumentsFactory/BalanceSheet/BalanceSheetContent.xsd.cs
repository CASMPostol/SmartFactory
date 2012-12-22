using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName )
    {
      return DocumentNames.CreateXmlFile<BalanceSheetContent>( destinationCollection, fileName, this, DocumentNames.BalanceSheetContentName );
    }
  }
}
