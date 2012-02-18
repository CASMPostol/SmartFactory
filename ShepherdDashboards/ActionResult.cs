using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  internal class ActionResult : List<string>
  {
    #region public
    internal void AddException(Exception _excptn)
    {
      string _frmt = "The operation has been interrupted by error {0}.";
      Add(String.Format(_frmt, _excptn.Message));
    }
    internal bool Valid { get { return this.Count == 0; } }
    public new void Add(string _source)
    {
      string _msg = String.Format("{0} must be provided", _source);
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _msg));
    }
    #endregion
  }

}
