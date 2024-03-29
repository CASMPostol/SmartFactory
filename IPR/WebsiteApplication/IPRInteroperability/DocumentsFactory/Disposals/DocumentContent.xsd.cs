﻿using System;
using CAS.SharePoint.DocumentsFactory;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.Disposals
{
  /// <summary>
  /// Compensatiion good enum
  /// </summary>
  public enum CompensatiionGood
  {
    /// <summary>
    /// The dust compensatiion good
    /// </summary>
    Dust,
    /// <summary>
    /// The waste  compensatiion good
    /// </summary>
    Waste,
    /// <summary>
    /// The cartons  compensatiion good
    /// </summary>
    Cartons,
    /// <summary>
    /// The tobacco  compensatiion good
    /// </summary>
    Tobacco
  }
  public partial class DocumentContent
  {

    /// <summary>
    /// Adds the document to collection <see cref="SPFileCollection" />.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="compensatiionGood">The compensatiion good.</param>
    /// <returns>
    /// An object of <see cref="SPFile" /> containing the serialized <paramref name="destinationCollection" />
    /// </returns>
    public SPFile AddDocument2Collection( SPWeb site, string fileName, string listName, CompensatiionGood compensatiionGood )
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
        case CompensatiionGood.Cartons:
          stylesheetName = DocumentNames.CartonsFormStylesheetName;
          break;
        case CompensatiionGood.Tobacco:
          stylesheetName = DocumentNames.TobaccoFormStylesheetName;
          break;
      }
      return File.CreateXmlFile<DocumentContent>( site, this, fileName, listName, stylesheetName );
    }
  }
}
