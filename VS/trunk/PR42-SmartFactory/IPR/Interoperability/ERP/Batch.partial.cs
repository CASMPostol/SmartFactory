//<summary>
//  Title   : Class representing the Batch message
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using CAS.SharePoint;

namespace CAS.SmartFactory.xml.erp
{
  /// <summary>
  /// Class representing the Batch message
  /// </summary>
  public partial class Batch
  {
    static public Batch ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(Batch));
        return (Batch)serializer.Deserialize(reader);
      }
    }
    public void Validate( string pattern, List<string> _validationErrors )
    {
      foreach ( BatchMaterial _material in Material.AsEnumerable<BatchMaterial>() )
        _material.Batch = _material.Batch.GetFirstCapture( pattern, String.Empty, _validationErrors );
    }
  }
}
