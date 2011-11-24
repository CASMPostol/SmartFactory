﻿using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.IPR
{
  public partial class Batch
  {
    static public Batch ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(typeof(Batch));
        return (Batch)invoice.Deserialize(reader);
      }
    }

  }
}
