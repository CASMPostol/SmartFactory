using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CAS.SmartFactory.xml.CELINA.SAD;
using System.Xml;

namespace CAS.SmartFactory.xml
{
  /// <summary>
  /// Represents any custom document from xml file.
  /// </summary>
  [Serializable()]
  public abstract class CustomsDocument
  {
    static public CustomsDocument ImportDocument(System.IO.Stream documetStream)
    {
      Type type = null;
      using (XmlReader reader = XmlReader.Create(documetStream))
      {
        if (reader.MoveToContent() != XmlNodeType.Element)
          throw new ApplicationException("The file does not contain valid xml file.");
        if (reader.Name.Contains("SAD"))
          type = typeof(SAD);
        else if (reader.Name.Contains("IE529"))
          type = typeof(ECS.IE529.IE529);
        else if (reader.Name.Contains("CLNE"))
          type = typeof(CELINA.CLNE.CLNE);
        else if (reader.Name.Contains("PZC"))
          type = typeof(CELINA.PZC.PZC);
      }
      if (type == null)
        throw new ApplicationException("The file does not contain a valid customs declaration xml document");
      documetStream.Seek(0, System.IO.SeekOrigin.Begin);
      using (XmlReader reader = XmlReader.Create(documetStream, new XmlReaderSettings() { }))
      {
        XmlSerializer invoice = new XmlSerializer(type);
        return (CustomsDocument)invoice.Deserialize(reader);
      }
    }

    public abstract string GetNrWlasny();
    public abstract string GetReferenceNumber();
    public abstract decimal GetItemNo(int index);
    public abstract int GoodsTableLength();
  }
}
