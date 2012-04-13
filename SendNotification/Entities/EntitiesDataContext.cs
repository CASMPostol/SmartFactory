using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.Shepherd.SendNotification.Entities
{
  public partial class EntitiesDataContext
  {
    public void ResolveChangeConflicts(ActionResult _rsult)
    {
      string _cp = "Starting";
      try
      {
        foreach (ObjectChangeConflict _itx in this.ChangeConflicts)
        {
          _cp = "ObjectChangeConflict";
          string _tmp = String.Format("Object: {0}", _itx.Object == null ? "null" : _itx.Object.ToString());
          if (_itx.MemberConflicts != null)
          {
            string _ft = ", Conflicts: Member.Name={0}; CurrentValue={1}; DatabaseValue={2}; OriginalValue={3}";
            String _chnges = String.Empty;
            foreach (MemberChangeConflict _mid in _itx.MemberConflicts)
            {
              _chnges += String.Format(_ft,
                _mid.Member == null ? "null" : _mid.Member.Name,
                _mid.CurrentValue == null ? "null" : _mid.CurrentValue.ToString(),
                _mid.DatabaseValue == null ? "null" : _mid.DatabaseValue.ToString(),
                _mid.OriginalValue == null ? "null" : _mid.OriginalValue.ToString());
            }
            _tmp += _chnges;
          }
          else
            _tmp += "; No member details";
          _rsult.AddMessage("ResolveChangeConflicts at: " + _cp, _tmp);
          _cp = "AddMessage";
          _itx.Resolve(RefreshMode.KeepCurrentValues);
        } //foreach (ObjectChangeConflict
      }
      catch (Exception ex)
      {
        string _frmt = "The current operation has been interrupted in ResolveChangeConflicts at {0} by error {1}.";
        throw new ApplicationException(String.Format(_frmt, _cp, ex.Message));
      }
    }
  }
}
