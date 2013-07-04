using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR
{
  [Serializable]
  internal class IPRDataConsistencyException : ApplicationException
  {
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors" )]
    public IPRDataConsistencyException(string source, string message, Exception innerException, string comments)
      : base(message, innerException)
    {
      Source = source;
      Comments = comments;
    }
    internal void Add2Log(Entities _edc)
    {
      ActivityLogCT.WriteEntry( _edc, this.Source, this.Message );
    }
    private IPRDataConsistencyException()
      : base()
    { }
  }
}
