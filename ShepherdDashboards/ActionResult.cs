using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  internal class ActionResult : List<string>
  {
    #region public

    internal bool Valid { get { return this.Count == 0; } }
    internal void AddException(Exception _excptn)
    {
      string _msg = String.Format("The operation interrupted by error {0}.", _excptn.Message);
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _msg));
    }
    public new void Add(string _source)
    {
      string _msg = String.Format("{0} must be provided.", _source);
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _msg));
    }
    #endregion
  }

}
