using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.Entities
{
  public partial class DistributionList
  {
    internal static string GetEmail(ShepherdRole _ccRole, EntitiesDataContext _EDC)
    {
      var _ccdl = (from _ccx in _EDC.DistributionList
                   where _ccx.ShepherdRole.GetValueOrDefault(Entities.ShepherdRole.Invalid) == _ccRole
                   select new { Email = _ccx.EMail }).FirstOrDefault();
      if (_ccdl == null || String.IsNullOrEmpty(_ccdl.Email))
        _ccdl = (from _ccx in _EDC.DistributionList
                 where _ccx.ShepherdRole.GetValueOrDefault(Entities.ShepherdRole.Invalid) == Entities.ShepherdRole.Administrator
                 select new { Email = _ccx.EMail }).FirstOrDefault();
      return _ccdl == null ? CommonDefinition.UnknownEmail : _ccdl.Email.UnknownIfEmpty();
    }
  }
}
