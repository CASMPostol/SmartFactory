//<summary>
//  Title   : RoutesCatalog
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System.IO;
using System.Xml.Serialization;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.InputData
{
  public partial class RoutesCatalog
  {
    /// <summary>
    /// Imports the document.
    /// </summary>
    /// <param name="documentStream">The document stream.</param>
    /// <returns></returns>
    static public RoutesCatalog ImportDocument( Stream documentStream )
    {
      XmlSerializer serializer = new XmlSerializer( typeof( RoutesCatalog ) );
      return (RoutesCatalog)serializer.Deserialize( documentStream );
    }
  }
}
