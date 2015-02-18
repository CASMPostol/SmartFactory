//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  /// <summary>
  /// Class SADCollection.
  /// </summary>
  [System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.cas.eu/SmartFactory.Customs.Messages.CELINA.SAD.xsd")]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.cas.eu/SmartFactory.Customs.Messages.CELINA.SAD.xsd", IsNullable = false)]
  public class SADCollection
  {
    private SAD[] _ListOfSAD;

    /// <summary>
    /// Gets or sets the list of sad.
    /// </summary>
    /// <value>The list of sad.</value>
    public SAD[] ListOfSAD
    {
      get { return _ListOfSAD; }
      set { _ListOfSAD = value; }
    }

  }
}
