using System.IO;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.Dictionaries
{
  public partial class Configuration
  {
    static public Configuration ImportDocument(Stream documetStream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
      return (Configuration)serializer.Deserialize(documetStream);
    }
  }
}
