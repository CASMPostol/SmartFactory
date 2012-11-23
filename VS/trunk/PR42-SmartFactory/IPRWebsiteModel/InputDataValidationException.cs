using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel
{
  /// <summary>
  /// Input Data Validation Exception
  /// </summary>
  public class InputDataValidationException: ArgumentException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InputDataValidationException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the operation that caused the current exception.</param>
    public InputDataValidationException( string message, string paramName, List<string> errors )
      : base( message, paramName )
    {
      m_Errors = errors;
    }
    #region public
    public bool Valid { get { return m_Errors.Count == 0; } }
    /// <summary>
    /// Reports the action result.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="FileName">A string that contains the file name including the extension.</param>
    public void ReportActionResult( string url, string FileName )
    {
      if ( m_Errors.Count == 0 )
        return;
      try
      {
        using ( Entities _edc = new Entities( url ) )
        {
          string _title = "Input Data Validation Errors";
          string _msg = String.Format( "The import of the file {3} is interrapted at operation {0} because of: {1}. List of {2} problems are reported.", ParamName, Message, m_Errors.Count, FileName );
          _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = _title, Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          foreach ( string _err in m_Errors )
            _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = _title, Treść = _err, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          _edc.SubmitChanges();
        }
      }
      catch ( Exception ) { }
    }
    #endregion
    private List<string> m_Errors;
  }
}
