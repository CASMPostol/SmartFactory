using System;
using Microsoft.SharePoint;
namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  interface IUserDescriptor
  {
    string Company { get; }
    string User { get; }
  }
}
