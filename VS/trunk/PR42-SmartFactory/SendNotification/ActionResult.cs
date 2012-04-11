using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  internal class ActionResult : List<string>
  {
    #region public

    internal bool Valid { get { return this.Count == 0; } }
    internal void AddException(string _src, Exception _excptn)
    {
      string _msg = String.Format("The operation interrupted at {0} by the error: {1}.", _src, _excptn.Message);
      base.Add(_msg);
    }
    public void AddMessage(string _src, string _message)
    {
      string _msg = String.Format("The operation reports at {0} the problem: {1}.", _src, _message);
      base.Add(_message);
    }
    #endregion
  }
}
