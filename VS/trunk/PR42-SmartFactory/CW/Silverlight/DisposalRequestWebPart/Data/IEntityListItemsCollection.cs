using System;
using Microsoft.SharePoint.Client;
namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  interface IEntityListItemsCollection
  {
    void SubmitingChanges();
    object GetFieldLookupValue( Microsoft.SharePoint.Client.FieldLookupValue fieldLookupValue );
    FieldLookupValue GetFieldLookupValue( Object entity );

  }
}
