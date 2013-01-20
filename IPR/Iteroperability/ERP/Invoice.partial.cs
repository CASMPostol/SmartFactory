using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CAS.SmartFactory.xml.erp
{
  public partial class Invoice
  {
    static public Invoice ImportDocument(Stream documetStream)
    {
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(typeof(Invoice));
        return (Invoice)invoice.Deserialize(reader);
      }
    }
  }
}
