using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.Dictionaries
{
  public partial class Configuration
  {
    static public Configuration ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
        return (Configuration)serializer.Deserialize(reader);
      }
    }
  }
}
