using System;
namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  interface IUserDescriptor
  {
    string Company { get; }
    Microsoft.SharePoint.SPUser User { get; }
  }
}
