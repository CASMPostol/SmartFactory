using System;
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
    /// <param name="errors">The list of errors.</param>
    public InputDataValidationException( string message, string paramName, ErrorsList errors )
      : base( message, paramName )
    {
      m_Errors = errors;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="InputDataValidationException" /> class.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="paramName">The name of the operation that caused the current exception.</param>
    /// <param name="error">The error to be reported.</param>
    /// <param name="fatal">if set to <c>true</c> the list contain fatal erros.</param>
    public InputDataValidationException( string message, string paramName, string error, bool fatal )
      : base( message, paramName )
    {
      m_Errors = new ErrorsList();
      m_Errors.Add( error, fatal );
    }
    #region public
    /// <summary>
    /// Gets a value indicating whether this <see cref="InputDataValidationException" /> is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if valid; otherwise, <c>false</c>.
    /// </value>
    public Result Valid
    {
      get
      {
        if ( m_Errors.Fatal )
          return Result.FatalErrors;
        if ( m_Errors.Count == 0 )
          return Result.Warnings;
        return Result.Success;
      }
    }
    public enum Result
    {
      /// <summary>
      /// The success
      /// </summary>
      Success,
      /// <summary>
      /// The warnings
      /// </summary>
      Warnings,
      /// <summary>
      /// The fatal errors
      /// </summary>
      FatalErrors
    }
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
          string _msg = String.Format( "The import of the file {3} encountered a problem at operation <b>{0}</b> because of: {1}. List of {2} problems are reported.", ParamName, Message, m_Errors.Count, FileName );
          _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = _title, Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          foreach ( string _err in m_Errors )
            _edc.ActivityLog.InsertOnSubmit( new ActivityLogCT() { Title = _title, Treść = _err, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) } );
          _edc.SubmitChanges();
        }
      }
      catch ( Exception ) { }
    }
    #endregion

    #region private
    private ErrorsList m_Errors;
    #endregion

  }
}
