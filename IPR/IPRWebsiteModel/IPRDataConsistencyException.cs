//<summary>
//  Title   : IPR Data Consistency Exception class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel
{
  /// <summary>
  /// IPR Data Consistency Exception class
  /// </summary>
  public class IPRDataConsistencyException: ApplicationException
  {
    #region public
    /// <summary>
    /// Gets the comments.
    /// </summary>
    public string Comments { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRDataConsistencyException" /> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="comments">The comments.</param>
    public IPRDataConsistencyException( string source, string message, Exception innerException, string comments )
      : base( message, innerException )
    {
      Source = source;
      Comments = comments;
    }
    /// <summary>
    /// Add the log entry with exception description.
    /// </summary>
    /// <param name="edc">The edc.</param>
    public void Add2Log( Entities edc )
    {
      ActivityLogCT.WriteEntry( edc, this.Source, this.Message );
    }
    #endregion

    #region private
    private IPRDataConsistencyException()
      : base()
    { }
    #endregion

  }
}
