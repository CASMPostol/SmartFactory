using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using CAS.SharePoint;
using System;

namespace CAS.SmartFactory.xml.erp
{
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
    public void Validate( string pattern, System.Collections.Generic.List<string> _validationErrors )
    {
      foreach ( BatchMaterial _material in Material.AsEnumerable<BatchMaterial>() )
        _material.Batch = _material.Batch.GetFirstCapture( pattern, String.Empty, _validationErrors );
    }
  }
}
