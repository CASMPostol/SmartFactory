using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml
{
  /// <summary>
  /// Imput Data Error Exception
  /// </summary>
  public class ImputDataErrorException: ApplicationException
  {
    /// <summary>
    /// Gets the source header.
    /// </summary>
    public string SourceHeader { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="ImputDataErrorException"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ImputDataErrorException(string source, string message, Exception innerException): base(message, innerException)
    {
      SourceHeader = source;
    }
    private ImputDataErrorException(): base()
    {}
  }
}
