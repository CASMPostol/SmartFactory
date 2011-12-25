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
              from idx in edc.JTIPartner
              where idx.ShepherdUserTitle.Contains(_user.Name)
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
    internal static Partner GetAtIndex(EntitiesDataContext edc, string _index)
    {
      if (String.IsNullOrEmpty(_index))
        throw new ApplicationException("Partner not found because the index is null");
      int _intIndex = int.Parse(_index);
      try
      {
        return (
              from idx in edc.JTIPartner
              where idx.Identyfikator == _intIndex
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException("Partner not found");
      }
    }
  }
}
