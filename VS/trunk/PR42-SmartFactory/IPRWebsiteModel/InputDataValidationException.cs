using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public InputDataValidationException( string message, string paramName )
      : base( message, paramName )
    { }
    #region public
    public bool Valid { get { return m_Errors.Count == 0; } }
    public void ReportActionResult( string _url )
    {
      if ( m_Errors.Count == 0 )
        return;
      try
      {
        using ( Entities _edc = new Entities( _url ) )
        {
          string _msg = String.Format( "The operation {0} is interrapted because of: {1}. List of {2} problems are reported.", ParamName, Message, m_Errors.Count );
          _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = "Inmput Data Validation Errors", Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          foreach ( string _err in m_Errors )
            _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = "ReportActionResult", Treść = _err, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          _edc.SubmitChanges();
        }
      }
      catch ( Exception ) { }
    }
    public void AddMessage( string _message )
    {
      m_Errors.Add( _message );
    }
    #endregion
    private List<string> m_Errors = new List<string>();
  }
}
