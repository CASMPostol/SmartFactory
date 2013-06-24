using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.erp
{
  public abstract class SKU
  {
    static public SKU ImportDocument(System.IO.Stream documetStream)
    {
      Type type = null;
      using (XmlReader reader = XmlReader.Create(documetStream))
      {
        if (reader.MoveToContent() != XmlNodeType.Element)
          throw new ImputDataErrorException(m_Source, "The file does not contain valid xml file.", null);
        if (reader.Name.Contains("Cutfiller"))
          type = typeof(Cutfiller);
        else if (reader.Name.Contains("Cigarettes"))
          type = typeof(Cigarettes);
      }
      if (type == null)
        throw new ImputDataErrorException(m_Source, "The file does not contain a valid SKU xml document", null);
      documetStream.Seek(0, System.IO.SeekOrigin.Begin);
      XmlSerializer serializer = new XmlSerializer(type);
      return (SKU)serializer.Deserialize(documetStream);
    }
    public enum SKUType { Cigarettes, Cutfiller }
    public abstract SKUType Type { get; }
    public abstract Material[] GetMaterial();
    private const string m_Source = "SKU processing";
  }
}
