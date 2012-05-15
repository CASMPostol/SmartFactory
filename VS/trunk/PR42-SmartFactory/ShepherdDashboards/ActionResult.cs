using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  /// <summary>
  /// Action Result
  /// </summary>
  public class ActionResult : List<string>
  {
    #region public

    /// <summary>
    /// Gets a value indicating whether this <see cref="ActionResult"/> is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if no error encountered.; otherwise, <c>false</c>.
    /// </value>
    internal bool Valid { get { return this.Count == 0; } }
    internal void AddException(Exception _excptn)
    {
      string _msg = String.Format("ReportExceptionTemplate".GetLocalizedString(), _excptn.Message);
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _msg));
    }
    public void AddLabel(string _source)
    {
      string _msg = _source + "MustBeProvided".GetLocalizedString();
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _msg));
    }
    public void AddMessage(string _message)
    {
      base.Add(String.Format(GlobalDefinitions.ErrorMessageFormat, _message));
    }
    #endregion
  }

}
