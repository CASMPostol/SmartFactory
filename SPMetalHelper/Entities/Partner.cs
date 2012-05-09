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
      if (edc.Partner == null)
        return null;
      else
        return edc.Partner.FirstOrDefault(idx => idx.ShepherdUserTitle.IsNullOrEmpty() ? false : idx.ShepherdUserTitle.Contains(_user.Name));
    }
  }
}
