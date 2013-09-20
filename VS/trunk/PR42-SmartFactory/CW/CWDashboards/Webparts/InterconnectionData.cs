//<summary>
//  Title   : InterconnectionData class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Dashboards.Webparts
{
  /// <summary>
  /// InterconnectionData class provides functionality to consume webpatrs interconnection.
  /// </summary>
  /// <typeparam name="DerivedType">The type of the erived type.</typeparam>
  internal abstract class InterconnectionData<DerivedType>: CAS.SharePoint.Web.InterconnectionData<DerivedType>
      where DerivedType: InterconnectionData<DerivedType>
  {
    internal string ID { get { return GetFieldValue( Element.IDColunmName ); } }
    internal string Title { get { return GetFieldValue( Element.TitleColunmName ); } }
  }
  internal class DisposalRequestInterconnectionData: InterconnectionData<DisposalRequestInterconnectionData>
  {
    internal DisposalRequestInterconnectionData()
      : base()
    { }
  }
}
