using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace CAS.SmartFactory.SPMetalHelper.XSD
{
  public partial class PRWeb
  {
    internal static void ImportDocument(Stream documetStream, PRWeb _object)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PRWeb));
      serializer.Serialize(documetStream, _object);
    }
  }
}
