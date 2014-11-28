using System.IO;
using System.Xml.Serialization;
using  CAS.SmartFactory.Shepherd.Client.Management.UpdateData;

namespace  CAS.SmartFactory.Shepherd.Client.Management.InputData
{
  public partial class RoutesCatalog
  {
    /// <summary>
    /// Imports the document.
    /// </summary>
    /// <param name="documetStream">The document stream.</param>
    /// <returns></returns>
    static public RoutesCatalog ImportDocument( Stream documetStream )
    {
      XmlSerializer serializer = new XmlSerializer( typeof( RoutesCatalog ) );
      return (RoutesCatalog)serializer.Deserialize( documetStream );
    }
  }
}
