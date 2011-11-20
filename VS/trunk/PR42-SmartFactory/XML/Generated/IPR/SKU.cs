using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.IPR
{
  public abstract class SKU
  {
    static public SKU ImportDocument(System.IO.Stream documetStream)
    {
      Type type = null;
      using (XmlReader reader = XmlReader.Create(documetStream))
      {
        if (reader.MoveToContent() != XmlNodeType.Element)
          throw new ApplicationException("The file does not contain valid xml file.");
        if (reader.Name.Contains("Cutfiller"))
          type = typeof(Cutfiller);
        else if (reader.Name.Contains("Cigarettes"))
          type = typeof(Cigarettes);
      }
      if (type == null)
        throw new ApplicationException("The file does not contain a valid SKU xml document");
      documetStream.Seek(0, System.IO.SeekOrigin.Begin);
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(type);
        return (SKU)invoice.Deserialize(reader);
      }
    }
  }
}
