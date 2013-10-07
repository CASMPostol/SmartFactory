//<summary>
//  Title   : interface IEntityListItemsCollection
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  interface IEntityListItemsCollection
  {

    void SubmitingChanges( System.EventHandler executeQuery );
    object GetFieldLookupValue( Microsoft.SharePoint.Client.FieldLookupValue fieldLookupValue );
    FieldLookupValue GetFieldLookupValue( Object entity );

  }
}
