using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR
{
  internal class IPRDataConsistencyException : ApplicationException
  {
    /// <summary>
    /// Gets the comments.
    /// </summary>
    public string Comments { get; private set; }
    /// <summary>
    /// Initializes a new instance of the <see cref="ImputDataErrorException"/> class.
    /// </summary>
    /// <param name="_source">The source.</param>
    /// <param name="_message">The message.</param>
    /// <param name="_innerException">The inner exception.</param>
    public IPRDataConsistencyException(string _source, string _message, Exception _innerException, string _comments )
      : base(_message, _innerException)
    {
      Source = _source;
      Comments = _comments;
    }
    private IPRDataConsistencyException()
      : base()
    { }

  }
}
