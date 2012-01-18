using System.IO;
using System.Xml.Serialization;

namespace CAS.SmartFactory.Shepherd.ImportExport.XML
{
  public partial class PreliminaryData
  {
    static public PreliminaryData ImportDocument(Stream documetStream)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PreliminaryData));
      return (PreliminaryData)serializer.Deserialize(documetStream);
    }
  }
}
