//<summary>
//  Title   : interface IEntityRef
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
  interface IEntityRef
  {
    void SetEntity( object entity );
    void SetLookup( FieldLookupValue value, DataContext dataContext, string listName );
    FieldLookupValue GetLookup( DataContext dataContext, string listName );
  }
}
