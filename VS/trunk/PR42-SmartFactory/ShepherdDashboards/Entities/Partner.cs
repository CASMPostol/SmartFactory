using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Partner
  {
    internal static Partner FindForUser(EntitiesDataContext edc, SPUser _user)
    {
      try
      {
        return (
              from idx in edc.Partner
              where idx.ShepherdUserTitle.Contains(_user.Name)
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
