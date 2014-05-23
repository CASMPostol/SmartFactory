//<summary>
//  Title   : DisposalRequest XmlSerializer class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.CW.Interoperability.ERP
{
  /// <summary>
  /// DisposalRequest XmlSerializer class
  /// </summary>
  public partial class DisposalRequest
  {
    public static DisposalRequest ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(DisposalRequest));
        return (DisposalRequest)serializer.Deserialize(reader);
      }
    }
  }
}
