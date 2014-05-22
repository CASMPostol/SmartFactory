using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.erp
{
  public partial class Stock
  {
    static public Stock ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(typeof(Stock));
        return (Stock)invoice.Deserialize(reader);
      }
    }
  }
}
