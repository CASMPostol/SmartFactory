using System;
namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  interface IEntityRef
  {
    void SetEntity( object entity );
    void SetLookup( Microsoft.SharePoint.Client.FieldLookupValue value, DataContext dataContext, string listName );
  }
}
